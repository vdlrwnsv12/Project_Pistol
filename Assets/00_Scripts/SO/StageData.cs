using UnityEngine;

namespace WhiteChoco
{
    #region StageData : 스테이지 관련 설정 데이터
    /// <summary>
    /// 각 스테이지마다 고유하게 설정되는 데이터 정보 클래스
    /// </summary>
    [CreateAssetMenu(fileName = "StageData", menuName = "Stage/StageData", order = 0)]
    public class StageData : ScriptableObject
    {
        #region Time

        /// <summary>
        /// 스테이지 제한 시간 (초 단위)
        /// </summary>
        [Header("스테이지 제한 시간")]
        public float stageTimeLimit = 30f;

        /// <summary>
        /// 경고 상태 진입 임계값 (초과시 텍스트 색상 변경)
        /// </summary>
        [Tooltip("예: 남은 시간이 10초 이하일 때 경고 색상 전환")]
        public float warningThreshold = 10f;

        #endregion

        #region UI Color

        /// <summary>
        /// 기본 타이머 텍스트 색상
        /// </summary>
        public Color defaultColor = Color.white;

        /// <summary>
        /// 경고 상태 타이머 색상
        /// </summary>
        public Color warningColor = Color.red;

        #endregion

        #region (추후 확장 항목 예시)

        /*
        /// <summary>
        /// 생성할 타겟 수
        /// </summary>
        public int targetCount;

        /// <summary>
        /// 스테이지 배경 프리팹
        /// </summary>
        public GameObject backgroundPrefab;

        /// <summary>
        /// 스테이지 BGM
        /// </summary>
        public AudioClip stageBGM;
        */

        #endregion
    }
    #endregion
}

