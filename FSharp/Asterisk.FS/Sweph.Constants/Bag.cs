using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweph.Constants
{
    public class Bag
    {
        public const int SE_ASC = 0;
        public const int SE_MC = 1;
        public const int SE_ARMC = 2;
        public const int SE_VERTEX = 3;
        public const int SE_EQUASC = 4;	/* "equatorial ascendant" */
        public const int SE_COASC1 = 5;	/* "co-ascendant" (W. Koch) */
        public const int SE_COASC2 = 6;	/* "co-ascendant" (M. Munkasey) */
        public const int SE_POLASC = 7;	/* "polar ascendant" (M. Munkasey) */
        public const int SE_NASCMC = 8;

        public const int SE_JUL_CAL = 0;
        public const int SE_GREG_CAL = 1;

        public const int SE_SIDM_FAGAN_BRADLEY = 0;
        public const int SE_SIDM_LAHIRI = 1;
        public const int SE_SIDM_DELUCE = 2;
        public const int SE_SIDM_RAMAN = 3;
        public const int SE_SIDM_USHASHASHI = 4;
        public const int SE_SIDM_KRISHNAMURTI = 5;
        public const int SE_SIDM_DJWHAL_KHUL = 6;
        public const int SE_SIDM_YUKTESHWAR = 7;
        public const int SE_SIDM_JN_BHASIN = 8;
        public const int SE_SIDM_BABYL_KUGLER1 = 9;
        public const int SE_SIDM_BABYL_KUGLER2 = 10;
        public const int SE_SIDM_BABYL_KUGLER3 = 11;
        public const int SE_SIDM_BABYL_HUBER = 12;
        public const int SE_SIDM_BABYL_ETPSC = 13;
        public const int SE_SIDM_ALDEBARAN_15TAU = 14;
        public const int SE_SIDM_HIPPARCHOS = 15;
        public const int SE_SIDM_SASSANIAN = 16;
        public const int SE_SIDM_GALCENT_0SAG = 17;
        public const int SE_SIDM_J2000 = 18;
        public const int SE_SIDM_J1900 = 19;
        public const int SE_SIDM_B1950 = 20;
        public const int SE_SIDM_USER = 255;
        public const int SE_SIDBIT_ECL_T0 = 256;

        public const long SEFLG_JPLEPH = 1L;	            // use JPL ephemeris 
        public const long SEFLG_SWIEPH = 2L;	            // use SWISSEPH ephemeris, default
        public const long SEFLG_MOSEPH = 4L;	            // use Moshier ephemeris 

        public const long SEFLG_HELCTR = 8L;	            // return heliocentric position 
        public const long SEFLG_TRUEPOS = 16L;	            // return true positions, not apparent 
        public const long SEFLG_J2000 = 32L;	            // no precession, i.e. give J2000 equinox 
        public const long SEFLG_NONUT = 64L;	            // no nutation, i.e. mean equinox of date 
        public const long SEFLG_SPEED3 = 128L;	            // speed from 3 positions (do not use it, SEFLG_SPEED is faster and preciser.) 

        public const long SEFLG_SPEED = 256L;	            // high precision speed (analyt. comp.)
        public const long SEFLG_NOGDEFL = 512L;	            // turn off gravitational deflection 
        public const long SEFLG_NOABERR = 1024L;	        // turn off 'annual' aberration of light 
        public const long SEFLG_EQUATORIAL = 2048L;	        // equatorial positions are wanted 
        public const long SEFLG_XYZ = 4096L;	            // cartesian, not polar, coordinates 
        public const long SEFLG_RADIANS = 8192L;	        // coordinates in radians, not degrees 
        public const long SEFLG_BARYCTR = 16384L;	        // barycentric positions 
        public const long SEFLG_TOPOCTR = (32 * 1024L);	    // topocentric positions 
        public const long SEFLG_SIDEREAL = (64 * 1024L);	// sidereal positions 

        /* planet numbers for the ipl parameter in swe_calc() */

        public const int SE_ECL_NUT = -1;
        public const int SE_SUN = 0;
        public const int SE_MOON = 1;
        public const int SE_MERCURY = 2;
        public const int SE_VENUS = 3;
        public const int SE_MARS = 4;
        public const int SE_JUPITER = 5;
        public const int SE_SATURN = 6;
        public const int SE_URANUS = 7;
        public const int SE_NEPTUNE = 8;
        public const int SE_PLUTO = 9;
        public const int SE_MEAN_NODE = 10;
        public const int SE_TRUE_NODE = 11;
        public const int SE_MEAN_APOG = 12;
        public const int SE_OSCU_APOG = 13;
        public const int SE_EARTH = 14;
        public const int SE_CHIRON = 15;
        public const int SE_PHOLUS = 16;
        public const int SE_CERES = 17;
        public const int SE_PALLAS = 18;
        public const int SE_JUNO = 19;
        public const int SE_VESTA = 20;
        public const int SE_INTP_APOG = 21;
        public const int SE_INTP_PERG = 22;

        public const int SE_NPLANETS = 23;
        public const int SE_FICT_OFFSET = 40;
        public const int SE_NFICT_ELEM = 15;

        /* Hamburger or Uranian "planets" */

        public const int SE_CUPIDO = 40;
        public const int SE_HADES = 41;
        public const int SE_ZEUS = 42;
        public const int SE_KRONOS = 43;
        public const int SE_APOLLON = 44;
        public const int SE_ADMETOS = 45;
        public const int SE_VULKANUS = 46;
        public const int SE_POSEIDON = 47;

        /* other fictitious bodies */

        public const int SE_ISIS = 48;
        public const int SE_NIBIRU = 49;
        public const int SE_HARRINGTON = 50;
        public const int SE_NEPTUNE_LEVERRIER = 51;
        public const int SE_NEPTUNE_ADAMS = 52;
        public const int SE_PLUTO_LOWELL = 53;
        public const int SE_PLUTO_PICKERING = 54;

        public const int SE_AST_OFFSET = 10000;

        public const int EX_HOUSE_SYS_P = 'P'; //Placidus
        public const int EX_HOUSE_SYS_K = 'K'; //Koch
        public const int EX_HOUSE_SYS_O = 'O'; //Porphyrius
        public const int EX_HOUSE_SYS_R = 'R'; //Regiomontanus
        public const int EX_HOUSE_SYS_C = 'C'; //Campanus
        public const int EX_HOUSE_SYS_A = 'A'; //or ‘E’	Equal (cusp 1 is Ascendant)
        public const int EX_HOUSE_SYS_V = 'V'; //Vehlow equal (Asc. in middle of house 1)
        public const int EX_HOUSE_SYS_W = 'W'; //Whole sign 
        public const int EX_HOUSE_SYS_X = 'X'; //axial rotation system
        public const int EX_HOUSE_SYS_H = 'H'; //azimuthal or horizontal system
        public const int EX_HOUSE_SYS_T = 'T'; //Polich/Page (“topocentric” system)
        public const int EX_HOUSE_SYS_B = 'B'; //Alcabitus
        public const int EX_HOUSE_SYS_M = 'M'; //Morinus
        public const int EX_HOUSE_SYS_U = 'U'; //Krusinski-Pisa
        public const int EX_HOUSE_SYS_G = 'G'; //Gauquelin sectors

        // appended; uncategorized

        // for swe_rise_transit() and swe_rise_transit_true_hor() 
        public const int SE_CALC_RISE = 1;
        public const int SE_CALC_SET = 2;
        /// <summary>
        /// upper meridian transit (southern for northern geo. latitudes)
        /// </summary>
        public const int SE_CALC_MTRANSIT = 4;
        /// <summary>
        /// lower meridian transit (northern, below the horizon)
        /// </summary>
        public const int SE_CALC_ITRANSIT = 8;

        /* the following bits can be added (or’ed) to SE_CALC_RISE or SE_CALC_SET */
        /// <summary>
        /// for rising or setting of disc center
        /// </summary>
        public const int SE_BIT_DISC_CENTER = 256;
        /// <summary>
        /// for rising or setting of lower limb of disc
        /// </summary>
        public const int SE_BIT_DISC_BOTTOM = 8192;
        /// <summary>
        /// if refraction is not to be considered
        /// </summary>
        public const int SE_BIT_NO_REFRACTION = 512;
        /// <summary>
        /// in order to calculate civil twilight
        /// </summary>
        public const int SE_BIT_CIVIL_TWILIGHT = 1024;
        /// <summary>
        /// in order to calculate nautical twilight
        /// </summary>
        public const int SE_BIT_NAUTIC_TWILIGHT = 2048;
        /// <summary>
        /// in order to calculate astronomical twilight
        /// </summary>
        public const int SE_BIT_ASTRO_TWILIGHT = 4096;
        /// <summary>
        /// neglect the effect of distance on disc size
        /// </summary>
        public const int SE_BIT_FIXED_DISC_SIZE = (16 * 1024);
    }
}
