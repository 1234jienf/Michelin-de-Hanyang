using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[CreateAssetMenu(fileName = "ItemDropTable", menuName = "ScriptableObjects/ItemDropTable", order = 0)]
public class SOItemDropTable : ScriptableObject {
    // 아이템 드롭 스텟
    [Serializable]
    public class ItemDropStat {
        public SOItem item; // 드롭되는 아이템
        // 가중치
        // (floor(weight/100)개는 확정 드롭 + (weight mod 100)% 확률로 추가 드롭)
        public float weight; 
    }

    // 드롭 테이블 
    [SerializeField]
    private List<ItemDropStat> itemDropStats = new List<ItemDropStat>();

    // 생성할 아이템 리스트를 생성
    public List<Tuple<SOItem, int>> CreateItemList() {
        List<Tuple<SOItem, int>> ret = new List<Tuple<SOItem, int>>();

        foreach (ItemDropStat itemDropStat in itemDropStats) {
            // 생성할 아이템 수
            int quantity = Mathf.FloorToInt(itemDropStat.weight/100) 
                + (UnityEngine.Random.Range(0f, 100f) <= itemDropStat.weight % 100 ? 1 : 0);
            
            ret.Add(new Tuple<SOItem, int>(itemDropStat.item, quantity));
        }

        return ret;
    }

    // 아이템 드롭
    public void DropItem(Tilemap tilemap, Vector3 position) {
        // 드롭 테이블에 따라 드롭할 아이템 목록 생성
        List<Tuple<SOItem, int>> items = CreateItemList();
        
        // 아이템 드롭
        foreach ((SOItem item, int quantity) in items) {
            for (int i = 0; i < quantity; i++) {
                int loopCnt = 0;
                // 드롭에 성공할 때 까지 반복
                while (true) {
                    if(loopCnt > 1000) {
                        Debug.Log("드롭 실패");
                        break;
                    }

                    // 드롭 위치 정하기
                    // position을 기준으로 반경 1 이하의 랜덤한 위치에 아이템 드롭
                    float distance = UnityEngine.Random.Range(0f, 1f);
                    float angle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
                    Vector3 delta = new(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0f);

                    // 드롭 위치에 타일이 있으면 성공
                    // 성공 시 다음 아이템으로 넘어간다
                    Vector3Int cellPos = tilemap.WorldToCell(position + delta);
                    if (tilemap.GetTile(cellPos) != null) {
                        Instantiate(item.prefab, position + delta, Quaternion.identity);
                        break;
                    }

                    loopCnt++;
                }
            }
        }
    }
}
