using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSelf : MonoBehaviour
{
    public void ActiveOnSelf()
    {
        this.gameObject.SetActive(true);
    }

    public void ActiveOffSelf()
    {
        this.gameObject.SetActive(false);
    }

    /*
     * @breif 사라지는 이펙트가 active false 할때 사용 (풀에 다시 넣는 용도)
     */
    public void ActiveOffEffPool()
    {
        GameMng.I.endEff(EFF_TYPE.EFF, this.gameObject);
    }

    /*
     * @breif 사라지는 이펙트가 active false 할때 사용 (풀에 다시 넣는 용도)
     */
    public void ActiveOffBackEffPool()
    {
        GameMng.I.endEff(EFF_TYPE.BACK_EFF, this.gameObject);
    }

    /*
     * @breif 사라지는 이펙트가 active false 할때 사용 (풀에 다시 넣는 용도)
     */
    public void ActiveOffRemoveEffPool()
    {
        GameMng.I.endEff(EFF_TYPE.REMOVE_EFF, this.gameObject);
    }
}
