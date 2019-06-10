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
        public EnemyClass characterClass;
        Sprite newSprite;
        string spriteName;
        SpriteRenderer spriteRenderer;
        Sprite[] subSprites;

        private void Start()
        {
            Enemy enemy = GetComponent<Enemy>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer&&enemy)
            {
                characterClass = enemy.characterClass;
                subSprites = Resources.LoadAll<Sprite>("Characters/" + characterClass.ToString());
            }
            else
            {
                Debug.LogError(GetType() + " couldn't find one of its required components");
                enabled = false;
            }
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
