using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FriedSynapse.FlowEnt.Editor
{
    internal class PreviewerWindow : FlowEntWindow<PreviewerWindow>
    {
        private const float DefaultTimelessEchoTimeout = 3f;
        private const string SelectMessage = "Please Select an object from the hierarchy first.";

        private enum MemberType
        {
            Field,
            Property,
            Method
        }

        private class AnimationInfo
        {
            public AnimationInfo(string name, MemberType type, AbstractAnimation animation)
            {
                this.name = name;
                this.type = type;
                this.animation = animation;
                if (animation.PlayState != PlayState.Building)
                {
                    animation.Stop();
                    animation.Reset();
                }
            }

            internal string name;
            internal MemberType type;
            internal AbstractAnimation animation;
        }

        protected override string Name => "FlowEnt Previewer";

        private const BindingFlags DefaultBindingFlags =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static readonly Type abstractAnimationType = typeof(AbstractAnimation);
        private static readonly Type abstractAnimationBuilderType = typeof(IAbstractAnimationBuilder);
        private static readonly object[] emptyArray = { };

        private TextElement label;
        private Button exitFocusButton;
        private ScrollView animationsElement;

        private Func<AbstractAnimation> GetFocusedAnimation { get; set; }

        protected override void CreateGUI()
        {
            LoadHeader();
            LoadContent();
            label = Content.Query<TextElement>("name").First();
            exitFocusButton = Content.Query<Button>("exitFocus").First();
            animationsElement = Content.Query<ScrollView>("animations").First();
            Bind();
            Selection.selectionChanged += RefreshAnimations;
            EditorApplication.playModeStateChanged += _ => RefreshAnimations();
            RefreshAnimations();
        }

        private void Bind()
        {
            exitFocusButton.clicked += ExitFocus;
        }

        internal void RefreshAnimations()
        {
            if (GetFocusedAnimation != null)
            {
                RenderFocusedAnimation();
            }
            else
            {
                RenderSelectedAnimations();
            }
        }

        internal void FocusAnimation(Func<AbstractAnimation> getAnimation)
        {
            exitFocusButton.visible = true;
            GetFocusedAnimation = getAnimation;
            RenderFocusedAnimation();
        }

        private void ExitFocus()
        {
            exitFocusButton.visible = false;
            GetFocusedAnimation = null;
        }

        private void RenderFocusedAnimation()
        {
            InitRender();
            RenderAnimationsInternal(new List<AnimationInfo>
            {
                new AnimationInfo("", MemberType.Field, GetFocusedAnimation())
            });
        }

        private void RenderSelectedAnimations()
        {
            InitRender();

            Transform transform = Selection.activeTransform;
            label.text = transform == null ? SelectMessage : transform.name;

            if (transform == null)
            {
                return;
            }

            RenderAnimationsInternal(GetAnimations(transform));
        }

        private void InitRender()
        {
            if (PreviewController.IsRunning)
            {
                PreviewController.Stop();
            }

            animationsElement.Clear();
        }

        private void RenderAnimationsInternal(List<AnimationInfo> animationsInfo)
        {
            foreach (AnimationInfo animationInfo in animationsInfo)
            {
                VisualElement animationElement = new VisualElement();
                animationElement.AddToClassList("animation");
                animationElement.AddToClassList(animationInfo.type.ToClassName());
                TextElement label = new TextElement
                {
                    text = $"{animationInfo.name} [{animationInfo.animation}]",
                };
                label.AddToClassList("label");
                animationElement.Add(label);
                PreviewableControlSection controlSection = new PreviewableControlSection();
                controlSection.Init(animationInfo.animation);
                animationElement.Add(controlSection);
                animationsElement.contentContainer.Add(animationElement);
            }
        }

        private List<AnimationInfo> GetAnimations(Transform transform)
        {
            List<AnimationInfo> animations = new List<AnimationInfo>();
            foreach (MonoBehaviour behaviour in transform.GetComponents<MonoBehaviour>())
            {
                animations.AddRange(
                    behaviour
                        .GetType()
                        .GetFields(DefaultBindingFlags)
                        .Where(fi => abstractAnimationBuilderType.IsAssignableFrom(fi.FieldType))
                        .Select(fi => new AnimationInfo(
                            char.ToUpper(fi.Name[0]) + fi.Name.Substring(1),
                            MemberType.Field,
                            ((IAbstractAnimationBuilder)fi.GetValue(behaviour)).Build()))
                        .ToList());

                animations.AddRange(
                    behaviour
                        .GetType()
                        .GetProperties(DefaultBindingFlags)
                        .Where(pi
                            => abstractAnimationType.IsAssignableFrom(pi.PropertyType)
                               && ((AbstractAnimation)pi.GetValue(behaviour))?.Stop() != null)
                        .Select(pi => new AnimationInfo(pi.Name,
                            MemberType.Property,
                            ((AbstractAnimation)pi.GetValue(behaviour)).Stop()))
                        .ToList());

                animations.AddRange(
                    behaviour
                        .GetType()
                        .GetMethods(DefaultBindingFlags)
                        .Where(mi
                            => !mi.IsSpecialName
                               && abstractAnimationType.IsAssignableFrom(mi.ReturnType)
                               && mi.GetParameters().Length == 0
                               && ((AbstractAnimation)mi.Invoke(behaviour, emptyArray))?.Stop() != null)
                        .Select(mi => new AnimationInfo(mi.Name,
                            MemberType.Method,
                            ((AbstractAnimation)mi.Invoke(behaviour, emptyArray)).Stop()))
                        .ToList());

                //TODO fix the de-focus reset bug - create a reset button
            }

            foreach (AnimationInfo animationInfo in animations)
            {
                switch (animationInfo.animation)
                {
                    case Echo echo:
                        if (echo.Timeout == null)
                        {
                            echo.SetTimeout(DefaultTimelessEchoTimeout);
                        }

                        break;
                }
            }

            return animations;
        }
    }
}