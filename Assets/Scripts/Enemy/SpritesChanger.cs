using System;
using UnityEngine;

namespace MyRPGGame.Enemies
{
    public enum EnemyClass
    {
        knight, bowman, mage, spearman
    }
    public class SpritesChanger : MonoBehaviour
    {
        private EnemyClass characterClass;
        private Sprite newSprite;
        private string spriteName;
        private SpriteRenderer spriteRenderer;
        private Sprite[] subSprites;

        private void Start()
        {
            Enemy enemy = GetComponent<Enemy>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            characterClass = enemy.characterClass;
            subSprites = Resources.LoadAll<Sprite>("Characters/" + characterClass.ToString());
        }

        void LateUpdate()
        {
            spriteName = spriteRenderer.sprite.name;

            newSprite = Array.Find(subSprites, item => item.name == spriteName);
            if (newSprite)
            {
                spriteRenderer.sprite = newSprite;
            }
        }
    }

}
