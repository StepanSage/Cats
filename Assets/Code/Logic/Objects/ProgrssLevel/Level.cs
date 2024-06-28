using UnityEngine;

namespace Code.ProgressLevel 
{
    public class Level : MonoBehaviour
    {
        private const int MAX_COUNT_STAR = 3;
        private const int AMOUNT_COMPIETED = 2;
       
        public bool IsCompleted { get; private set;}
        public int ID { get; private set; }
        public int AmountStar { get; private set; }

        public void Initialized(int id, int amountStar)
        {
            IsCompleted = false;
            ID = id;

           

            if (amountStar <= MAX_COUNT_STAR)
            {
                AmountStar = amountStar;
            }
            else
            {
                DebugX.LogError("Count star in level the amount is more than allowed");
            }

            if(amountStar >= AMOUNT_COMPIETED)
            {
                IsCompleted = true;
            }
        }

    }
}

