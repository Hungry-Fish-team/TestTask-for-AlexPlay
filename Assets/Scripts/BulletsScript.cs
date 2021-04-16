using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsScript : MonoBehaviour
{
    [SerializeField]
    private int bulletIndex;
    [SerializeField]
    private int bulletDamage;
    [SerializeField]
    private int bulletSpeed;
    [SerializeField]
    private int bulletForce;
    [SerializeField]
    private Vector2 endPosition;

    [SerializeField]
    GameObject parent;

    [SerializeField]
    GameObject bulletEndEffect;

    BulletEffects[] bulletEffects;

    public class BulletEffects : MonoBehaviour
    {
        private int bulletDamage;
        private int bulletSpeed;
        private int bulletForce;

        public virtual void AddBulletPar(int bulletDamage, int bulletSpeed, int bulletForce)
        {
            this.bulletDamage = bulletDamage;
            this.bulletSpeed = bulletSpeed;
            this.bulletForce = bulletForce;
        }

        public virtual void DestroyBullet(Collider2D collision, GameObject parent, GameObject bullet)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        Destroy(bullet);
                    }
                }
            }
        }

        public virtual void TakeDamage(Collider2D collision, GameObject parent, GameObject bullet)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        collision.GetComponent<Player>().TakeDamageFormAnotherPlayer(bulletDamage);
                    }
                }
            }
        }

        public virtual void TakeForceFromBulletToEnemy(Collider2D collision, GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        collision.GetComponent<Rigidbody2D>().AddForce(CalcNewForceVector(collision.gameObject, bullet) * bulletForce);
                    }
                }
            }
        }

        public Vector2 CalcNewForceVector(GameObject collision, GameObject bullet)
        {
            Vector2 forsePos;

            if (bullet.transform.position.x < collision.transform.position.x)
            {
                if (collision.transform.localScale.x > 0)
                {
                    forsePos = new Vector2(collision.transform.position.x + 5f, collision.transform.position.y);
                }
                else
                {
                    forsePos = new Vector2(collision.transform.position.x + 5f, collision.transform.position.y);
                }
            }
            else
            {
                if (collision.transform.localScale.x > 0)
                {
                    forsePos = new Vector2(collision.transform.position.x - 5f, collision.transform.position.y);
                }
                else
                {
                    forsePos = new Vector2(collision.transform.position.x + 5f, collision.transform.position.y);
                }
            }

            return forsePos;
        }
    }

    public class BulletSlowMove : BulletEffects
    {
        private int bulletDamage;
        private int bulletSpeed;
        private int bulletForce;


        public override void AddBulletPar(int bulletDamage, int bulletSpeed, int bulletForce)
        {
            this.bulletDamage = bulletDamage;
            this.bulletSpeed = bulletSpeed;
            this.bulletForce = bulletForce;
        }

        public override void TakeDamage(Collider2D collision, GameObject parent, GameObject bullet)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        collision.GetComponent<Player>().TakeDamageFormAnotherPlayer(bulletDamage);
                    }
                }
            }
        }

        public override void DestroyBullet(Collider2D collision, GameObject parent, GameObject bullet)
        {
            if (collision.gameObject != parent)
            {
                if (collision.CompareTag("Player"))
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        Destroy(bullet);
                    }
                }
                else
                {
                    Destroy(bullet);
                }
            }
        }

        public override void TakeForceFromBulletToEnemy(Collider2D collision, GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {                
                        collision.GetComponent<Rigidbody2D>().AddForce(CalcNewForceVector(collision.gameObject, bullet) * bulletForce);
                    }
                }
            }
        }
    }

    public class BulletExplosion : BulletEffects
    {
        private int bulletDamage;
        private int bulletSpeed;
        private int bulletForce;

        public override void AddBulletPar(int bulletDamage, int bulletSpeed, int bulletForce)
        {
            this.bulletDamage = bulletDamage;
            this.bulletSpeed = bulletSpeed;
            this.bulletForce = bulletForce;
        }

        public override void DestroyBullet(Collider2D collision, GameObject parent, GameObject bullet)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        //Destroy(bullet);
                    }
                }
            }
        }

        public override void TakeDamage(Collider2D collision, GameObject parent, GameObject bullet)
        {

        }

        public override void TakeForceFromBulletToEnemy(Collider2D collision, GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            StartCoroutine(StartExposion(parent, bullet, bulletEndEffect));
        }

        public IEnumerator StartExposion(GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            yield return new WaitForSeconds(2f);
            FindLocalPlayer(parent, bullet, bulletEndEffect);
        }

        private void FindLocalPlayer(GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            List<GameObject> playerInRadius = new List<GameObject>();
            int radius = 8;

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                if (Vector2.Distance(player.transform.position, bullet.transform.position) <= radius)
                {
                    if (player != parent)
                    {
                        if (parent.GetComponent<Player>().ReturnPlayerColor() != player.GetComponent<Player>().ReturnPlayerColor())
                        {
                            playerInRadius.Add(player);
                        }
                    }
                }
            }

            MakeBoom(playerInRadius, parent, bullet, bulletEndEffect);
        }

        private void MakeBoom(List<GameObject> playerInRadius, GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            StartCoroutine(StartEndEffect(bulletEndEffect, bullet));

            foreach (GameObject player in playerInRadius)
            {
                if (parent.GetComponent<Player>().ReturnPlayerColor() != player.GetComponent<Player>().ReturnPlayerColor())
                {
                    player.GetComponent<Rigidbody2D>().AddForce(CalcNewForceVector(player, bullet) * bulletForce);
                    player.GetComponent<Player>().TakeDamageFormAnotherPlayer(bulletDamage);
                }
            }
        }

        IEnumerator StartEndEffect(GameObject bulletEndEffect, GameObject bullet)
        {
            GameObject endBulletEffectObj = Instantiate(bulletEndEffect, bullet.transform.position, Quaternion.identity, bullet.transform);
            //Debug.Log("CreateEffect");
            yield return new WaitForSeconds(0.4f);
            Destroy(endBulletEffectObj);
            Destroy(bullet);
        }
    }

    public class BulletSniper : BulletEffects
    {
        private int bulletDamage;
        private int bulletSpeed;
        private int bulletForce;

        public override void AddBulletPar(int bulletDamage, int bulletSpeed, int bulletForce)
        {
            this.bulletDamage = bulletDamage;
            this.bulletSpeed = bulletSpeed;
            this.bulletForce = bulletForce;
        }

        public override void DestroyBullet(Collider2D collision, GameObject parent, GameObject bullet)
        {

        }

        public override void TakeDamage(Collider2D collision, GameObject parent, GameObject bullet)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        collision.GetComponent<Player>().TakeDamageFormAnotherPlayer(bulletDamage);
                    }
                }
            }
        }

        public override void TakeForceFromBulletToEnemy(Collider2D collision, GameObject parent, GameObject bullet, GameObject bulletEndEffect)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.gameObject != parent)
                {
                    if (parent.GetComponent<Player>().ReturnPlayerColor() != collision.GetComponent<Player>().ReturnPlayerColor())
                    {
                        collision.GetComponent<Rigidbody2D>().AddForce(CalcNewForceVector(collision.gameObject, bullet) * bulletForce);
                    }
                }
            }
        }
    }

    public void BulletMove(Vector2 endPosition, GameObject parent)
    {
        this.endPosition = endPosition;
        this.parent = parent;

        Vector2 direction = endPosition - (Vector2)transform.position;

        GetComponent<Rigidbody2D>().AddForce(direction.normalized * bulletSpeed, ForceMode2D.Impulse);
    }

    public void SetBulletDamage(int bulletDamage)
    {
        this.bulletDamage = bulletDamage;
    }

    public int ReturnBulletDamage()
    {
        return bulletDamage;
    }

    public void SetBulletSpeed(int bulletSpeed)
    {
        this.bulletSpeed = bulletSpeed;
    }

    public int ReturnBulletSpeed()
    {
        return bulletSpeed;
    }

    private void Start()
    {
        BulletSlowMove bulletSlowMove = gameObject.AddComponent<BulletSlowMove>();
        bulletSlowMove.AddBulletPar(bulletDamage, bulletSpeed, bulletForce);
        BulletExplosion bulletExplosion = gameObject.AddComponent<BulletExplosion>();
        bulletExplosion.AddBulletPar(bulletDamage, bulletSpeed, bulletForce);
        BulletSniper bulletSniper = gameObject.AddComponent<BulletSniper>();
        bulletSniper.AddBulletPar(bulletDamage, bulletSpeed, bulletForce);

        bulletEffects = new BulletEffects[]
        {
            bulletSniper,
            bulletSlowMove,
            bulletExplosion
        };
    }

    private int ChechBulletType()
    {
        switch (parent.GetComponent<Player>().ReturnPlayerClass())
        {
            case "Hunter":
                {
                    bulletIndex = 0;
                    break;
                }
            case "Captain":
                {
                    bulletIndex = 1;
                    break;
                }
            case "BigGuns":
                {
                    bulletIndex = 2;
                    break;
                }
        }

        return bulletIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        bulletEffects[ChechBulletType()].TakeForceFromBulletToEnemy(collision, parent, gameObject, bulletEndEffect);

        bulletEffects[ChechBulletType()].TakeDamage(collision, parent, gameObject);

        bulletEffects[ChechBulletType()].DestroyBullet(collision, parent, gameObject);
    }
}
