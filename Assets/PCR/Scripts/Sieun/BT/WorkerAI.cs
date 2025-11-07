//using PCR;
//using System.Collections.Generic;
//using UnityEngine;
//using static INode;

//[RequireComponent(typeof(Animator))]
//public class WorkerAI : MonoBehaviour
//{
//    [Header("")]
//    private BehaviorTreeRunner btRunner = null;
   
//    //private Animator animator = null;
//    //애니메이터, transfrom, 이벤트, 델리게이트(->to worker parameter)

//    private void Awake()
//    {
//        //animator = GetComponent<Animator>();
//        btRunner = new BehaviorTreeRunner(SettingBT());
//    }

//    private void OnEnable()
//    {
//        //Worker worker = new Worker();

//    }

//    private void OnDisable()
//    {
//    }

//    private void Update()
//    {
//        btRunner.Operate();
//    }

//    INode SettingBT()
//    {
//        return new SelectorNode
//        (
//            new SequenceNode
//            (
//               new List<INode>()
//               {
//                 new ActionNode(),
//                 new ActionNode(),
//                 new ActionNode(),
//                 new ActionNode(),
//               }
//            ),


//            new SelectorNode
//            (
//                new List<INode>()
//                {
//                    new SequenceNode
//                    (
//                        new ActionNode(),
//                        new ActionNode(),
//                        new ActionNode(),
//                    ),


//                     new SequenceNode
//                     (
//                         new List<INode>()
//                         {
//                              new ActionNode(),
//                              new ActionNode(),
//                              new ActionNode(),
//                         }

//                     ),


//                    new ActionNode(),
//                    }
//            ),

//            // (선택사항) 3. 모든 작업/휴식 외 최하위 행동 (예: 그냥 멍때리기)
//            // new ActionNode(Idle),
//        );
//    }

//    bool IsWorkerEating()
//    {

//    }

//    bool IsWorkerWorking()
//    {

//    }

//    INode.WorkerNodeState CheckEating()
//    {
//        //워커가 이벤트를 받을 수 있는지 worker.state를체크

//            return INode.WorkerNodeState.WNS_SUCESS;
//    }
//    INode.WorkerNodeState CheckWorking()
//    {

//    }

//    INode.WorkerNodeState GoToEatingPlace(//원래위치)
//    {
//        //원래위치를저장
//        //밥먹는의자로이동
//        EatingInRestaurant(//원래위치);
//    }

//    INode.WorkerNodeState EatingInRestaurant(//원래위치)
//    {
//        // 원래 있던 위치를 저장한다
//        // 밥을 정해진 시간동안 먹는다.(코루틴) 밥을 먹는 애니메이션을 실행한다.
//        // 원래 위치로 복귀(MoveToDestination(원래위치) 호출)
//    }

//    INode.WorkerNodeState MoveToDestination(//목적지 매개변수)
//    {
//        if (// 목적지 도착했다면)
//        {
//            // 목적지가 특정건물이면 UI 뷰 카메라에 작업자가 소환
            
//            // 캐릭터 mesh를 숨긴다
//        }
//    }





//    //NodeState
//    //액션의조건
//    //액션실제수행로직
//    //State결과리턴


//    //INode SettingBT()
//    //{
//    //    return new SelectorNode // or
//    //    (
//    //        new List<INode>()
//    //        {

//    //             new SequenceNode // 제1우선순위 : 워커의 배고픔
//    //             (
//    //                new List<INode>()
//    //                {
//    //                    new ActionNode(//워커가이미밥을먹고있는지체크),
//    //                    new ActionNode(//복귀할 수 있게 OriginPosition 저장. 기존에 하던 작업 있으면 그 작업을 저장. 작업관두기or멈추기델리게이트. MoveToDestination(식당) 호출)
//    //                    new ActionNode(//밥을 먹는다. 다 먹으면 MoveToDestination(OriginPosition, 원래하던일여부) 호출
//    //                }
//    //             ),

//    //             new SequenceNode
//    //             (
//    //                 new List<INode>()
//    //                 {
//    //                    new ActionNode(// 기존 위치로 도착하면, 하다가 멈춰놓은 작업을 다시 델리게이트로 += 연결. 하다가 멈춰놓은 작업이 없으면 MoveToDestination(응접실)로 이동한다.),
//    //                    new ActionNode(// 작업 명령이 새로 들어왔는지 체크한다. 확인되면 MoveToDestination(새 작업지) 호출),
//    //                    new ActionNode(// 작업이 완료되었는지 체크한다. 완료되면 MoveToDestination(응접실)로 이동한다.),
//    //                 }
//    //             ),
//    //             //new ActionNode(//명령된 장소로 이동.MoveToDestination),
//    //        }

//    //    );
//    //}

//}
