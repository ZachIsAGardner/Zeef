using System.Threading.Tasks;

namespace Zeef.TwoDimensional.Example 
{
    public class TestPlayerLivingObject: LivingObject 
    {
        private TestPlayer testPlayer;

        private void Awake() 
        {
            testPlayer = GetComponent<TestPlayer>();
        }

        public override async Task DieAsync() 
        {
            OnBeforeDie();
            testPlayer.Respawn();
        }
    }
}