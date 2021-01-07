using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CakeShop
{
    class ConstantVariable
    {
        public const double EPSILON = 1e-5;

        public const int UPDATE_TYPECAKE = 0;
        public const int ADD_TYPECAKE = 1;
        public const int DEL_TYPECAKE = 2;
        //public const int UPDATE_CAKE = 2;
        //public const int ADD_CAKE = 3;
        //public const int UPDATE_JOURNEY = 4;
        //public const int ADD_JOURNEY = 5;

        public const int RIBBON_TYPECAKE = 0;
        public const int RIBBON_CAKE = 1;

        public const int DURING_SPLASH_SCREEN = 10;

        public const int CANCEL_ACTION = -1;

        public static char[] separatingPathChars = { '/', '\\' };


        public const int MAX_DIMENSIONS_SPLASH = 450;
        public static double convertDimension(double size)
        {
            if (size <= MAX_DIMENSIONS_SPLASH)
            {
                return size;
            }
            else
            {
                return MAX_DIMENSIONS_SPLASH;
            }
        }


        private static Semaphore _syncLock = new Semaphore(1, 1);

        private static ConstantVariable _instance = null;

        public static ConstantVariable Variable
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConstantVariable();
                }
                return _instance;
            }
        }

        public static Semaphore SyncLock
        {
            get
            {
                return _syncLock;
            }
        }



        private ConstantVariable()
        {

        }
    }
}
