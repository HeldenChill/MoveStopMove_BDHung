using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomAttribute;

public enum PoolID
{
    Player = -1,
    Character = 0,
    #region Bullet
    Bullet_Axe1 = 1,
    Bullet_Knife1 = 2,
    Bullet_Axe2 = 3,
    Bullet_Arrow = 4,
    #endregion

    #region Weapon
    Weapon_Axe1 = 100,
    Weapon_Knife1 = 101,
    Weapon_Axe2 = 102,
    Weapon_Arrow = 103,
    #endregion

    #region Skins
    Hair_Arrow = 1000,
    Hair_Cowboy = 1001,
    Hair_Headphone = 1002,
    Hair_Ear = 1003,
    Hair_Crown = 1004,
    Hair_Horn = 1005,
    Hair_Beard = 1006,
    #endregion

    None = 10000,
    UIItem = 10001,
    UITargetIndicator = 10002,
    Obstance = 10003,
    Gift = 10004,
    BaseWeapon = 10005,
    ObjectCreateWeapon = 10006

    
}
namespace MoveStopMove.Manager
{
    using Photon.Pun;
    using Utilitys;

    [DefaultExecutionOrder(-1)]
    public class PrefabManager : Singleton<PrefabManager>
    {

        //NOTE:Specific for game,remove to reuse
        [SerializeField]
        GAMECONST.GAMEPLAY_MODE mode = GAMECONST.GAMEPLAY_MODE.STANDARD_PVE;
        private GameObject PrefabPool;
        [SerializeField]
        GameObject Character;
        #region Bullet
        [SerializeField]
        GameObject Bullet_Axe1;
        [SerializeField]
        GameObject Bullet_Knife1;
        [SerializeField]
        GameObject Bullet_Axe2;
        [SerializeField]
        GameObject Bullet_Arrow;
        #endregion
        #region Weapon
        [SerializeField]
        GameObject Weapon_Axe1;
        [SerializeField]
        GameObject Weapon_Knife1;
        [SerializeField]
        GameObject Weapon_Axe2;
        [SerializeField]
        GameObject Weapon_Arrow;
        #endregion
        #region Hair
        [SerializeField]
        GameObject Hair_Arrow;
        [SerializeField]
        GameObject Hair_Cowboy;
        [SerializeField]
        GameObject Hair_Headphone;
        [SerializeField]
        GameObject Hair_Ear;
        [SerializeField]
        GameObject Hair_Crown;
        [SerializeField]
        GameObject Hair_Horn;
        [SerializeField]
        GameObject Hair_Beard;
        #endregion
        [SerializeField]
        GameObject Obstance;
        
        [SerializeField]
        GameObject UIItem;
        [SerializeField]
        GameObject UIIndicator;
        [SerializeField]
        GameObject Gift;
        [SerializeField]
        GameObject BaseWeapon;
        [SerializeField]
        GameObject ObjectCreateWeapon;
        //-----
        [Separator]
        #region Bullet
        [SerializeField]
        GameObject Bullet_Axe1Pvp;
        [SerializeField]
        GameObject Bullet_Knife1Pvp;
        [SerializeField]
        GameObject Bullet_Axe2Pvp;
        [SerializeField]
        GameObject Bullet_ArrowPvp;
        #endregion
        #region Weapon
        [SerializeField]
        GameObject Weapon_Axe1Pvp;
        [SerializeField]
        GameObject Weapon_Knife1Pvp;
        [SerializeField]
        GameObject Weapon_Axe2Pvp;
        [SerializeField]
        GameObject Weapon_ArrowPvp;
        #endregion
        [Separator]
        [SerializeField]
        GameObject pool;
        [Separator]
        [SerializeField]
        PhotonPrefabManager photon;
        private PrefabManager pvePrefabManager;       
        Dictionary<PoolID, Pool> poolData = new Dictionary<PoolID, Pool>();
        List<KeyValuePair<int, int>> serializeData = new List<KeyValuePair<int, int>>();
        protected override void Awake()
        {          
            if(mode == GAMECONST.GAMEPLAY_MODE.STANDARD_PVP)
            {               
                pvePrefabManager = inst;
                inst = null;              
                Debug.Log("Prefab Manager PvP Instantiate!");
                photon.SetDataSerialize(ref serializeData);
            }
            base.Awake();

            if (mode == GAMECONST.GAMEPLAY_MODE.STANDARD_PVE)
                SpawnObjectPvE();

        }
        public void CreatePool(GameObject obj, PoolID namePool, Quaternion quaternion = default, int numObj = 10, bool network = false)
        {
            GameObject newPool;
            if(network == false)
            {
                newPool = Instantiate(pool, Vector3.zero, Quaternion.identity, transform);
                newPool.transform.parent = PrefabPool.transform;
            }
            else
            {
                newPool = NetworkManager.Inst.Instantiate(pool.name);
                newPool.transform.parent = transform;
            }
            
            Pool poolScript = newPool.GetComponent<Pool>();
            poolScript.IsSetParent = true;
            newPool.name = namePool.ToString();

            if (!poolData.ContainsKey(namePool))
            {
                poolScript.Initialize(obj, quaternion, numObj, network);
                poolData.Add(namePool, poolScript);
            }
            else
            {
                
                poolScript.Initialize(obj, quaternion, numObj, network);
                Destroy(poolData[namePool].gameObject);
                poolData[namePool] = poolScript;
            }

            serializeData.Clear();           
        }
        public void PushToPool(GameObject obj, PoolID namePool, bool checkContain = true)
        {
            if (!poolData.ContainsKey(namePool))
            {
                switch (mode)
                {
                    case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                        CreatePool(obj, namePool, default, 10, true);
                        break;
                    case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                        CreatePool(obj, namePool);
                        break;
                }
                
            }

            poolData[namePool].Push(obj, checkContain);
        }
        public GameObject PopFromPool(PoolID namePool, GameObject obj = null)
        {
            if (!poolData.ContainsKey(namePool))
            {
                if (obj == null)
                {
                    Debug.LogError("No pool name " + namePool + " was found!!!");
                    return null;
                }
            }

            return poolData[namePool].Pop();
        }

        public void SpawnObjectPvP()
        {
            PrefabPool = Instantiate(pool);
            PrefabPool.name = "PrefabPoolPvp";
            CreatePool(Bullet_Axe1Pvp, PoolID.Bullet_Axe1, Quaternion.Euler(0, 0, 0), 10, true);
            CreatePool(Bullet_Knife1Pvp, PoolID.Bullet_Knife1, Quaternion.Euler(0, 0, 0), 10, true);
            CreatePool(Bullet_Axe2Pvp, PoolID.Bullet_Axe2, Quaternion.Euler(0, 0, 0), 10, true);
            CreatePool(Bullet_ArrowPvp, PoolID.Bullet_Arrow, Quaternion.Euler(0, 0, 0), 10, true);

            CreatePool(Weapon_Axe1Pvp, PoolID.Weapon_Axe1, Quaternion.Euler(0, 0, 0), 10, true);
            CreatePool(Weapon_Knife1Pvp, PoolID.Weapon_Knife1, Quaternion.Euler(0, 0, 0), 10, true);
            CreatePool(Weapon_Axe2Pvp, PoolID.Weapon_Axe2, Quaternion.Euler(0, 0, 0), 10, true);
            CreatePool(Weapon_ArrowPvp, PoolID.Weapon_Arrow, Quaternion.Euler(0, 0, 0), 10, true);

            CreatePool(Hair_Arrow, PoolID.Hair_Arrow);
            CreatePool(Hair_Cowboy, PoolID.Hair_Cowboy);
            CreatePool(Hair_Headphone, PoolID.Hair_Headphone);
            CreatePool(Hair_Ear, PoolID.Hair_Ear);
            CreatePool(Hair_Crown, PoolID.Hair_Crown);
            CreatePool(Hair_Horn, PoolID.Hair_Horn);
            CreatePool(Hair_Beard, PoolID.Hair_Beard);

            CreatePool(UIItem, PoolID.UIItem);
            CreatePool(UIIndicator, PoolID.UITargetIndicator);
            CreatePool(Obstance, PoolID.Obstance);
            CreatePool(Gift, PoolID.Gift);
            CreatePool(BaseWeapon, PoolID.BaseWeapon, Quaternion.identity, 5);
            CreatePool(ObjectCreateWeapon, PoolID.ObjectCreateWeapon, Quaternion.identity, 50);
            foreach (var i in poolData)
            {
                PhotonView photonView = i.Value.gameObject.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    serializeData.Add(new KeyValuePair<int, int>((int)i.Key, photonView.ViewID));
                }
            }

            DontDestroyOnLoad(PrefabPool);
        }

        public void SpawnObjectPvE()
        {
            PrefabPool = Instantiate(pool);
            PrefabPool.name = "PrefabPool";
            CreatePool(Character, PoolID.Character, Quaternion.Euler(0, 0, 0), 15);
            CreatePool(Bullet_Axe1, PoolID.Bullet_Axe1, Quaternion.Euler(0, 0, 0));
            CreatePool(Bullet_Knife1, PoolID.Bullet_Knife1, Quaternion.Euler(0, 0, 0));
            CreatePool(Bullet_Axe2, PoolID.Bullet_Axe2, Quaternion.Euler(0, 0, 0));
            CreatePool(Bullet_Arrow, PoolID.Bullet_Arrow, Quaternion.Euler(0, 0, 0));

            CreatePool(Weapon_Axe1, PoolID.Weapon_Axe1, Quaternion.Euler(0, 0, 0));
            CreatePool(Weapon_Knife1, PoolID.Weapon_Knife1, Quaternion.Euler(0, 0, 0));
            CreatePool(Weapon_Axe2, PoolID.Weapon_Axe2, Quaternion.Euler(0, 0, 0));
            CreatePool(Weapon_Arrow, PoolID.Weapon_Arrow, Quaternion.Euler(0, 0, 0));

            CreatePool(Hair_Arrow, PoolID.Hair_Arrow);
            CreatePool(Hair_Cowboy, PoolID.Hair_Cowboy);
            CreatePool(Hair_Headphone, PoolID.Hair_Headphone);
            CreatePool(Hair_Ear, PoolID.Hair_Ear);
            CreatePool(Hair_Crown, PoolID.Hair_Crown);
            CreatePool(Hair_Horn, PoolID.Hair_Horn);
            CreatePool(Hair_Beard, PoolID.Hair_Beard);

            CreatePool(UIItem, PoolID.UIItem);
            CreatePool(UIIndicator, PoolID.UITargetIndicator);
            CreatePool(Obstance, PoolID.Obstance);
            CreatePool(Gift, PoolID.Gift);
            CreatePool(BaseWeapon, PoolID.BaseWeapon, Quaternion.identity, 5);
            CreatePool(ObjectCreateWeapon, PoolID.ObjectCreateWeapon, Quaternion.identity, 50);

            DontDestroyOnLoad(PrefabPool);
        }

        public void ChangeMode(GAMECONST.GAMEPLAY_MODE mode)
        {
            if (this.mode == mode) return;
            switch (mode)
            {
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVE:
                    inst = pvePrefabManager;
                    pvePrefabManager.gameObject.SetActive(true);
                    Destroy(gameObject);
                    break;
                case GAMECONST.GAMEPLAY_MODE.STANDARD_PVP:
                    PrefabManager pvpPrefabManager = NetworkManager.Inst.Instantiate(gameObject.name).GetComponent<PrefabManager>();
                    pvpPrefabManager.SpawnObjectPvP();
                    pvpPrefabManager.pvePrefabManager = this;
                    gameObject.SetActive(false);
                    
                    break;
            }
        }
    }
}