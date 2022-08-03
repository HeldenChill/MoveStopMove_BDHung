using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys.AI
{
    using MoveStopMove.Core.Character.LogicSystem;
    public class IdleState : BaseState
    {
        public IdleState(StateMachine StateMachine,BasicStateInsts States,LogicParameter Parameter, LogicData Data, LogicEvent Event) 
            : base(StateMachine, States ,Parameter, Data, Event)
        {
           
        }
        public override void Enter()
        {
            base.Enter();
            //TODO: Play Animation 
            Event.SetVelocity(Vector3.zero);
            Event.SetBool_Anim(GameConst.ANIM_IS_IDLE, true);
        }

        public override void Exit()
        {
            base.Exit();
        }


        public override int LogicUpdate()
        {
            base.LogicUpdate();
            if (Parameter.Die)
            {
                StateMachine.ChangeState(States.GetState(State.Die));
            }
            else if (Parameter.CharacterPositions.Count > 0 && Data.AttackCount > 0)
            {
                StateMachine.ChangeState(States.GetState(State.Attack));
            }
            else if(Parameter.MoveDirection.sqrMagnitude > 0.001)
            {
                StateMachine.ChangeState(States.GetState(State.Move));
            }
            return 0;
        }


    }
}