using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieUI : MonoBehaviour
{
    [SerializeField] GameObject dungeonDieUI;
    [SerializeField] GameObject raidFailUI;
    // [SerializeField] GameObject dungeonClearUI;
    [SerializeField] GameObject mustDieUI;

    /**
     * @brief 던전에서 사망했을때
     */
    public void dungeonDie()
    {
        dungeonDieUI.SetActive(true);
    }

    /**
     * @brief 레이드시 전원 사망했을때
     */
    public void raidFail()
    {
        raidFailUI.SetActive(true);
        StartCoroutine(moveToRaidRepair());
    }

    IEnumerator moveToRaidRepair()
    {
        yield return new WaitForSeconds(5);

        raidFailUI.SetActive(false);
        if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_0))
            NetworkMng.I.changeRoom(ROOM_CODE.RAID_0_REPAIR);
        else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_1))
            NetworkMng.I.changeRoom(ROOM_CODE.RAID_1_REPAIR);
        else if (NetworkMng.I.myRoom.Equals(ROOM_CODE.RAID_2))
            NetworkMng.I.changeRoom(ROOM_CODE.RAID_2_REPAIR);
    }

    /**
     * @brief 던전클리어
     */
    // public void dungeonClear()
    // {
    //     dungeonClearUI.SetActive(true);
    // }

    /**
     * @brief 마을로 돌아가기
     */
    public void backToSquare()
    {
        NetworkMng.I.changeRoom(ROOM_CODE.HOME);
    }

    /**
     * @brief 즉시사망 (UI 애니메이션 실행)
     */
    public void mustDieAnim()
    {
        mustDieUI.SetActive(true);
    }
}
