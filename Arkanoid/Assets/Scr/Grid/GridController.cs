using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
    
    [SerializeField] private Vector2 _offset = new Vector2(-5.45f, 4);
    [SerializeField] private LevelData _currentLevelData;

    private void Start() {
        BuildGrid();
    }

    private void BuildGrid() {
        //Grid data
        int rowCount = _currentLevelData.RowCount;
        float verticalSpacing = _currentLevelData.rowSpacing;
       
        for (int j = 0; j < rowCount; j++) {
           GridRowData rowData = _currentLevelData.Rows[j];
            int blockCount = 7;
            float horizontalSpacing = 0.1f;
            Vector2 blockSize = new Vector2(1.5f, 0.5f);
            BlockTile blockTilePerfab = Resources.Load<BlockTile>("Prefabs/BigBlockTile");
            BlockColor blockColor = rowData.BlockColor;

            if (blockTilePerfab == null) {
                return;
            }

            for (int i = 0; i < blockCount; i++) {
                BlockTile blockTile = Instantiate<BlockTile>(blockTilePerfab, transform);
                float x = _offset.x + blockSize.x/2 + (blockSize.x + horizontalSpacing) * i;
                float y = _offset.y - (blockSize.y + verticalSpacing) * j;
                blockTile.transform.position = new Vector3(x, y, 0);
                   
                blockTile.SetData(blockColor);
                blockTile.Init();
            }
        }
    }

    private Vector2 GetBlockSize(BlockType type) {
        if (type == BlockType.Big) {
            return new Vector2(1.5f, 0.5f);
        }
        else if (type == BlockType.Small) {
            return new Vector2(0.75f, 0.25f); // aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
        }
        return Vector2.zero;
    }

    private string GetBlockPath(BlockType type) {
        if (type == BlockType.Big) {
            return "Prefabs/BigBlockTile";
        }
        else if (type == BlockType.Small) {
            return "Prefabs/SmallBlockTile";
        }
        return string.Empty;
    }
}
