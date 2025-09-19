using System;
using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace CCT.Script
{
    public enum ButtonType
    {
        Attack
    }

    public enum InputType
    {
        Tap,
        HoldPerformed,
        HoldCanceled
    }

    public struct FrameInputInfo
    {
        public ButtonType buttonType;
        public InputType inputType;
        public uint frameIndex;
    }

    public class InputComponent : MonoBehaviour
    {
        private InputParams _inputParams = new();

        public BehaviourTreeOwner btOwner;
        
        private uint _frameIndex;
        private Queue<FrameInputInfo> _frameInputs = new();
        private List<FrameInputInfo> _inputInfosToAnalyze = new(30);

        private float _interval = 1f / 30f;
        private float _lastAnalyzeTime;

        private void Update()
        {
            while (Time.time - _lastAnalyzeTime > _interval)
            {
                _lastAnalyzeTime += _interval;
                _inputInfosToAnalyze.Clear();
                while (_frameInputs.Count > 0 && _frameInputs.Peek().frameIndex == _frameIndex)
                {
                    _inputInfosToAnalyze.Add(_frameInputs.Dequeue());
                }
                _inputParams.Analyze(_inputInfosToAnalyze);
                // behaviour tree update
                btOwner.UpdateBehaviour();
                _frameIndex++;
            }
        }

        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if (ctx is { interaction: TapInteraction, performed: true })
            {
                _frameInputs.Enqueue(new FrameInputInfo()
                {
                    frameIndex = _frameIndex,
                    buttonType = ButtonType.Attack,
                    inputType = InputType.Tap,
                });
            }
            if (ctx.interaction is HoldInteraction)
            {
                if (ctx.performed)
                {
                    _frameInputs.Enqueue(new FrameInputInfo()
                    {
                        frameIndex = _frameIndex,
                        buttonType = ButtonType.Attack,
                        inputType = InputType.HoldPerformed,
                    });
                }
                else if (ctx.canceled)
                {
                    _frameInputs.Enqueue(new FrameInputInfo()
                    {
                        frameIndex = _frameIndex,
                        buttonType = ButtonType.Attack,
                        inputType = InputType.HoldCanceled,
                    });
                }
            }
        }
    }
}