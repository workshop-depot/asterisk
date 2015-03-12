using System;
using System.Runtime.InteropServices;
using System.Text;
using CentiSec = System.Int32;

namespace Sweph
{
    public class Sweph
    {
        #region 16.1. Calculation of planets and stars

        #region Planets, moon, asteroids, lunar nodes, apogees, fictitious bodies

        /// <summary>
        /// swe_calc_ut
        /// </summary>
        /// <param name="tjd_ut">Julian day, Universal Time</param>
        /// <param name="ipl">body number</param>
        /// <param name="iflag">a 32 bit integer containing bit flags that indicate what kind of computation is wanted</param>
        /// <param name="xx">array of 6 doubles for longitude, latitude, distance, speed in long., speed in lat., and speed in dist</param>
        /// <param name="serr">character string to return error messages in case of error</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_calc_ut")]
        public static extern int swe_calc_ut(double tjd_ut, int ipl, int iflag, double[] xx, StringBuilder serr);

        /// <summary>
        /// swe_calc
        /// </summary>
        /// <param name="tjd_et">Julian day, Ephemeris time,  where tjd_et = tjd_ut + swe_deltat(tjd_ut)</param>
        /// <param name="ipl">body number</param>
        /// <param name="iflag">a 32 bit integer containing bit flags that indicate what kind of computation is wanted</param>
        /// <param name="xx">array of 6 doubles for longitude, latitude, distance, speed in long., speed in lat., and speed in dist</param>
        /// <param name="serr">character string to return error messages in case of error</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_calc")]
        public static extern int swe_calc(double tjd_et, int ipl, int iflag, double[] xx, StringBuilder serr);

        #endregion

        #region Fixed stars

        /// <summary>
        /// 
        /// </summary>
        /// <param name="star">star name, returned star name 40 bytes</param>
        /// <param name="tjd_ut">Julian day number, Universal Time</param>
        /// <param name="iflag">flag bits</param>
        /// <param name="xx">target address for 6 position values: longitude, latitude, distance, long. speed, lat. speed, dist. speed</param>
        /// <param name="serr">256 bytes for error string</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_fixstar_ut")]
        public static extern long swe_fixstar_ut(
            StringBuilder star,
            double tjd_ut,
            long iflag,
            double[] xx,
            StringBuilder serr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="star">star name, returned star name 40 bytes</param>
        /// <param name="tjd_et">Julian day number, Ephemeris Time</param>
        /// <param name="iflag">flag bits</param>
        /// <param name="xx">target address for 6 position values: longitude, latitude, distance, long. speed, lat. speed, dist. speed</param>
        /// <param name="serr">256 bytes for error string</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_fixstar")]
        public static extern long swe_fixstar(
            StringBuilder star,
            double tjd_et,
            long iflag,
            double[] xx,
            StringBuilder serr);

        #endregion

        #region Set the geographic location for topocentric planet computation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geolon">geographic longitude</param>
        /// <param name="geolat">geographic latitude: eastern longitude is positive, western longitude is negative, northern latitude is positive, southern latitude is negative</param>
        /// <param name="altitude">altitude above sea</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_set_topo")]
        public static extern void swe_set_topo(double geolon, double geolat, double altitude);

        #endregion

        #region Set the sidereal mode for sidereal planet positions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sid_mode"></param>
        /// <param name="t0">reference epoch</param>
        /// <param name="ayan_t0">initial ayanamsha at t0</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_set_sid_mode")]
        public static extern void swe_set_sid_mode(int sid_mode, double t0, double ayan_t0);

        [DllImport("swedll32.dll", EntryPoint = "swe_get_ayanamsa_ut")]
        public static extern double swe_get_ayanamsa_ut(double tjd_ut);

        [DllImport("swedll32.dll", EntryPoint = "swe_get_ayanamsa")]
        public static extern double swe_get_ayanamsa(double tjd_et);

        #endregion

        #endregion

        #region 16.2 Eclipses and planetary phenomena

        //TODO:

        #endregion

        #region 16.3. Date and time conversion

        #region Delta T from Julian day number

        /// <summary>
        /// delta t from Julian day number
        /// </summary>
        /// <param name="tjd"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_deltat")]
        public static extern double swe_deltat(double tjd);

        #endregion

        #region Julian day number from year, month, day, hour, with check whether date is legal

        /// <summary>
        /// swe_date_conversion
        /// </summary>
        /// <param name="y">year</param>
        /// <param name="m">month</param>
        /// <param name="d">day</param>
        /// <param name="hour">hours (decimal, with fraction)</param>
        /// <param name="c">calendar ‘g’[regorian]|’j’[ulian]</param>
        /// <param name="tjd">return value for Julian day</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_date_conversion")]
        public static extern int swe_date_conversion(
            int y, int m, int d,    /* year, month, day */
            double hour, 			/* hours (decimal, with fraction) */
            char c,  			    /* calendar ‘g’[regorian]|’j’[ulian] */
            ref double tjd);		/* return value for Julian day */

        #endregion

        #region Julian day number from year, month, day, hour
        [DllImport("swedll32.dll", EntryPoint = "swe_julday")]
        public static extern double swe_julday(int year, int month, int day, double hour, int gregflag);
        #endregion

        #region Year, month, day, hour from Julian day number
        /// <summary>
        /// swe_revjul
        /// </summary>
        /// <param name="tjd">Julian day number</param>
        /// <param name="gregflag">Gregorian calendar: 1, Julian calendar: 0</param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        [DllImport("swedll32.dll", EntryPoint = "swe_revjul")]
        public static extern void swe_revjul(
            double tjd, 		/* Julian day number */
            int gregflag,		/* Gregorian calendar: 1, Julian calendar: 0 */
            ref int year,		/* target addresses for year, etc. */
            ref int month, ref  int day, ref  double hour);
        #endregion

        #region Local time to UTC and UTC to local time

        /*
        /// <summary>
        /// transform local time to UTC or UTC to local time
        /// input:
        ///   iyear ... dsec     date and time
        ///   d_timezone         timezone offset
        /// output:
        ///   iyear_out ... dsec_out
        /// For time zones east of Greenwich, d_timezone is positive.
        /// For time zones west of Greenwich, d_timezone is negative.
        /// For conversion from local time to utc, use +d_timezone.
        /// For conversion from utc to local time, use -d_timezone.
        /// </summary>
        [DllImport("swedll32.dll", EntryPoint = "swe_utc_timezone")]
        public static extern ulong swe_utc_timezone(
               Int32 iyear, Int32 imonth, Int32 iday,
               Int32 ihour, Int32 imin, double dsec,
               double d_timezone,
               ref Int32 iyear_out, ref Int32 imonth_out, ref Int32 iday_out,
               ref Int32 ihour_out, ref Int32 imin_out, ref double dsec_out);
        */

        #endregion

        #region UTC to jd (TT and UT1)
        /// <summary>
        /// input: date and time (wall clock time), calendar flag.
        /// output: an array of doubles with Julian Day number in ET (TT) and UT (UT1)
        ///             an error message (on error)
        /// The function returns OK or ERR.
        /// </summary>
        /// <param name="iyear"></param>
        /// <param name="imonth"></param>
        /// <param name="iday"></param>
        /// <param name="ihour"></param>
        /// <param name="imin"></param>
        /// <param name="dsec">note : second is a decimal</param>
        /// <param name="gregflag">Gregorian calendar: 1, Julian calendar: 0</param>
        /// <param name="dret">
        /// return array, two doubles:
        /// dret[0] = Julian day in ET (TT)
        /// dret[1] = Julian day in UT (UT1)
        /// </param>
        /// <param name="serr">error string</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_utc_to_jd")]
        public static extern void swe_utc_to_jd(
                 Int32 iyear, Int32 imonth, Int32 iday,
                 Int32 ihour, Int32 imin, double dsec,
                 int gregflag,
                 double[] dret,
                 StringBuilder serr);
        #endregion

        #region TT (ET1) to UTC
        /// <summary>
        /// input: Julian day number in ET (TT), calendar flag
        /// output: year, month, day, hour, min, sec in UTC 
        /// </summary>
        /// <param name="tjd_et">Julian day number in ET (TT)</param>
        /// <param name="gregflag">Gregorian calendar: 1, Julian calendar: 0</param>
        /// <param name="iyear"></param>
        /// <param name="imonth"></param>
        /// <param name="iday"></param>
        /// <param name="ihour"></param>
        /// <param name="imin"></param>
        /// <param name="dsec">note : second is a decimal</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_jdet_to_utc")]
        public static extern void swe_jdet_to_utc(
            double tjd_et,
            int gregflag,
            ref Int32 iyear, ref  Int32 imonth, ref  Int32 iday,
            ref  Int32 ihour, ref  Int32 imin, ref  double dsec);
        #endregion

        #region UTC to TT (ET1)
        /// <summary>
        /// input: Julian day number in UT (UT1), calendar flag
        /// output: year, month, day, hour, min, sec in UTC 
        /// </summary>
        /// <param name="tjd_ut">Julian day number in ET (TT)</param>
        /// <param name="gregflag">Gregorian calendar: 1, Julian calendar: 0</param>
        /// <param name="iyear"></param>
        /// <param name="imonth"></param>
        /// <param name="iday"></param>
        /// <param name="ihour"></param>
        /// <param name="imin"></param>
        /// <param name="dsec">note : second is a decimal</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_jdut1_to_utc")]
        public static extern void swe_jdut1_to_utc(
            double tjd_ut,
            int gregflag,
            ref Int32 iyear, ref Int32 imonth, ref Int32 iday,
            ref Int32 ihour, ref Int32 imin, ref double dsec);
        #endregion

        #region Get tidal acceleration used in swe_deltat()
        /// <summary>
        /// get tidal acceleration used in swe_deltat()
        /// </summary>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_get_tid_acc")]
        public static extern double swe_get_tid_acc();
        #endregion

        #region Set tidal acceleration to be used in swe_deltat()
        /* set tidal acceleration to be used in swe_deltat() */
        [DllImport("swedll32.dll", EntryPoint = "swe_set_tid_acc")]
        public static extern void swe_set_tid_acc(double t_acc);
        #endregion

        #region Equation of time
        /// <summary>
        /// function returns the difference between local apparent and local mean time.
        /// e = LAT – LMT.  tjd_et is ephemeris time 
        /// </summary>
        /// <param name="tjd_et"></param>
        /// <param name="e"></param>
        /// <param name="serr"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_time_equ")]
        public static extern int swe_time_equ(double tjd_et, ref double e, StringBuilder serr);
        #endregion

        #endregion

        #region 16.4. Initialization, setup, and closing functions

        #region Set directory path of ephemeris files
        [DllImport("swedll32.dll", EntryPoint = "swe_set_ephe_path")]
        public static extern int swe_set_ephe_path(string path);

        /// <summary>
        /// set name of JPL ephemeris file
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_set_jpl_file")]
        public static extern int swe_set_jpl_file(string fname);

        [DllImport("swedll32.dll", EntryPoint = "swe_close")]
        public static extern void swe_close();
        #endregion

        #endregion

        #region 16.5. House calculation

        #region Sidereal time
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tjd_ut">Julian day number, UT</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_sidtime")]
        public static extern double swe_sidtime(double tjd_ut);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tjd_ut">Julian day number, UT</param>
        /// <param name="eps">obliquity of ecliptic, in degrees</param>
        /// <param name="nut">nutation in longitude, in degrees</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_sidtime0")]
        public static extern double swe_sidtime0(
            double tjd_ut,
            double eps,
            double nut);
        #endregion

        #region House cusps, ascendant and MC
        /// <summary>
        /// house cusps, ascendant and MC
        /// </summary>
        /// <param name="tjd_ut">Julian day number, UT</param>
        /// <param name="geolat">geographic latitude, in degrees</param>
        /// <param name="geolon">
        /// geographic longitude, in degrees
        /// eastern longitude is positive,
        /// western longitude is negative,
        /// northern latitude is positive,
        /// southern latitude is negative   
        /// </param>
        /// <param name="hsys">house method, ascii code of one of the letters PKORCAEVXHTBG</param>
        /// <param name="cusps">array for 13 doubles</param>
        /// <param name="ascmc">array for 10 doubles</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_houses")]
        public static extern int swe_houses(
            double tjd_ut,
            double geolat,
            double geolon,
            int hsys,
            double[] cusps,
            double[] ascmc);
        #endregion

        #region Extended house function; to compute tropical or sidereal positions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="armc">ARMC</param>
        /// <param name="geolat">geographic latitude, in degrees</param>
        /// <param name="eps">ecliptic obliquity, in degrees</param>
        /// <param name="hsys">
        /// house method, ascii code of one of the letters PKORCAEVXHTBG
        /// hsys = 	‘P’	Placidus
        ///         ‘K’	Koch
        ///         ‘O’	Porphyrius
        ///         ‘R’	Regiomontanus
        ///         ‘C’	Campanus
        ///         ‘A’ or ‘E’	Equal (cusp 1 is Ascendant)
        ///         ‘V’	Vehlow equal (Asc. in middle of house 1)
        ///         ‘W’	Whole sign 
        ///         ‘X’	axial rotation system
        ///         ‘H’	azimuthal or horizontal system
        ///         ‘T’	Polich/Page (“topocentric” system)
        ///         ‘B’	Alcabitus
        ///         ‘M’	Morinus
        ///         ‘U’	Krusinski-Pisa
        ///         ‘G’	Gauquelin sectors
        /// </param>
        /// <param name="cusps">array for 13 doubles</param>
        /// <param name="ascmc">array for 10 doubles</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_houses_armc")]
        public static extern int swe_houses_armc(
            double armc,
            double geolat,
            double eps,
            int hsys,
            double[] cusps,
            double[] ascmc);

        /// <summary>
        /// extended function; to compute tropical or sidereal positions
        /// </summary>
        /// <param name="tjd_ut">Julian day number, UT</param>
        /// <param name="iflag">0 or SEFLG_SIDEREAL or SEFLG_RADIANS</param>
        /// <param name="geolat"></param>
        /// <param name="geolon">
        /// geographic latitude, in degrees  
        /// geographic longitude, in degrees
        /// eastern longitude is positive,
        /// western longitude is negative,
        /// northern latitude is positive,
        /// southern latitude is negative    
        /// </param>
        /// <param name="hsys">house method, ascii code of one of the letters PKORCAEVXHTBG</param>
        /// <param name="cusps">array for 13 doubles</param>
        /// <param name="ascmc">array for 10 doubles</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_houses_ex")]
        public static extern int swe_houses_ex(
            double tjd_ut,
            int iflag,
            double geolat,
            double geolon,
            int hsys,
            double[] cusps,
            double[] ascmc);
        #endregion

        #region Get the house position of a celestial point
        /// <summary>
        /// 
        /// </summary>
        /// <param name="armc">ARMC</param>
        /// <param name="geolat">geographic latitude, in degrees</param>
        /// <param name="eps">ecliptic obliquity, in degrees</param>
        /// <param name="hsys">house method, one of the letters PKRCAV</param>
        /// <param name="xpin">array of 2 doubles: ecl. longitude and latitude of the planet</param>
        /// <param name="serr">return area for error or warning message</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_house_pos")]
        public static extern double swe_house_pos(
            double armc,
            double geolat,
            double eps,
            int hsys,
            double[] xpin,
            StringBuilder serr);
        #endregion

        #region Get the Gauquelin sector position for a body
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tjd_ut">search after this time (UT)</param>
        /// <param name="ipl">planet number, if planet, or moon</param>
        /// <param name="starname">star name, if star</param>
        /// <param name="iflag">flag for ephemeris and SEFLG_TOPOCTR</param>
        /// <param name="imeth">
        /// method: 0 = with lat., 1 = without lat.,
        ///         2 = from rise/set, 3 = from rise/set with refraction
        /// </param>
        /// <param name="geopos">
        /// array of three doubles containing
        /// geograph. long., lat., height of observer
        /// </param>
        /// <param name="atpress">
        /// atmospheric pressure, only useful with imeth=3;
        /// if 0, default = 1013.25 mbar is used           
        /// </param>
        /// <param name="attemp">atmospheric temperature in degrees Celsius, only useful with imeth=3</param>
        /// <param name="dgsect">return address for gauquelin sector position</param>
        /// <param name="serr">return address for error message</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_gauquelin_sector")]
        public static extern double swe_gauquelin_sector(
            double tjd_ut,
            Int32 ipl,
            string starname,
            Int32 iflag,
            Int32 imeth,
            double[] geopos,
            double atpress,
            double attemp,
            double[] dgsect,
            StringBuilder serr);
        #endregion

        #endregion

        #region 16.6. Auxiliary functions

        #region Coordinate transformation, from ecliptic to equator or vice-versa
        /// <summary>
        /// equator -> ecliptic     : eps must be positive
        /// ecliptic -> equator       : eps must be negative eps, longitude and latitude are in degrees!
        /// </summary>
        /// <param name="xpo">3 doubles: long., lat., dist. to be converted; distance remains unchanged, can be set to 1.00</param>
        /// <param name="xpn">3 doubles: long., lat., dist. Result of the conversion</param>
        /// <param name="eps">obliquity of ecliptic, in degrees.</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_cotrans")]
        public static extern void swe_cotrans(
            double[] xpo,
            double[] xpn,
            double eps);
        #endregion

        #region Coordinate transformation of position and speed, from ecliptic to equator or vice-versa
        /// <summary>
        /// equator -> ecliptic        : eps must be positive
        /// ecliptic -> equator       : eps must be negative
        /// eps, long., lat., and speeds in long. and lat. are in degrees!
        /// </summary>
        /// <param name="xpo">6 doubles, input: long., lat., dist. and speeds in long., lat and dist.</param>
        /// <param name="xpn">6 doubles, position and speed in new coordinate system</param>
        /// <param name="eps">obliquity of ecliptic, in degrees.</param>
        [DllImport("swedll32.dll", EntryPoint = "swe_cotrans_sp")]
        public static extern void swe_cotrans_sp(
            double[] xpo,
            double[] xpn,
            double eps);
        #endregion

        #region Get the name of a planet
        [DllImport("swedll32.dll", EntryPoint = "swe_get_planet_name")]
        public static extern void swe_get_planet_name(int ipl, StringBuilder spname);

        /// <summary>
        /// normalization of any degree number to the range 0 ... 360
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_degnorm")]
        public static extern double swe_degnorm(double x);
        #endregion

        #endregion

        #region 16.7. Other functions that may be useful

        #region Normalize argument into interval [0..DEG360]
        /// <summary>
        /// former function name: csnorm()
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_csnorm")]
        public static extern CentiSec swe_csnorm(CentiSec p);
        #endregion

        #region Distance in centisecs p1 - p2 normalized to [0..360]
        /// <summary>
        /// former function name: difcsn()
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_difcsn")]
        public static extern CentiSec swe_difcsn(CentiSec p1, CentiSec p2);
        #endregion

        #region Distance in degrees
        /// <summary>
        /// former function name: difdegn() 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_difdegn")]
        public static extern double swe_difdegn(double p1, double p2);
        #endregion

        #region Distance in centisecs p1 - p2 normalized to [-180..180]
        /// <summary>
        /// former function name: difcs2n()
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_difcs2n")]
        public static extern CentiSec swe_difcs2n(CentiSec p1, CentiSec p2);
        #endregion

        #region Distance in degrees
        /// <summary>
        /// former function name: difdeg2n() 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_difdeg2n")]
        public static extern double swe_difdeg2n(double p1, double p2);
        #endregion

        #region Round second, but at 29.5959 always down
        /// <summary>
        /// former function name: roundsec()
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_csroundsec")]
        public static extern CentiSec swe_csroundsec(CentiSec x);
        #endregion

        #region Double to long with rounding, no overflow check
        /// <summary>
        /// former function name: d2l() 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_d2l")]
        public static extern long swe_d2l(double x);
        #endregion

        #region Day of week
        /// <summary>
        /// Monday = 0, ... Sunday = 6  former function name: day_of_week()
        /// </summary>
        /// <param name="jd"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_day_of_week")]
        public static extern int swe_day_of_week(double jd);
        #endregion

        #region Centiseconds -> time string
        /// <summary>
        /// former function name: TimeString()
        /// </summary>
        /// <param name="t"></param>
        /// <param name="sep"></param>
        /// <param name="suppressZero"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_cs2timestr")]
        public static extern string swe_cs2timestr(CentiSec t, int sep, bool suppressZero, string a);
        #endregion

        #region Centiseconds -> longitude or latitude string
        /// <summary>
        /// former function name: LonLatString()
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pchar"></param>
        /// <param name="mchar"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_cs2lonlatstr")]
        public static extern string swe_cs2lonlatstr(CentiSec t, char pchar, char mchar, string s);
        #endregion

        #region Centiseconds -> degrees string
        /// <summary>
        /// former function name: DegreeString()
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        [DllImport("swedll32.dll", EntryPoint = "swe_cs2degstr")]
        public static extern string swe_cs2degstr(CentiSec t, string a);
        #endregion

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tjd_ut">search after this time (UT)</param>
        /// <param name="ipl">planet number, if planet or moon</param>
        /// <param name="starname">star name, if star</param>
        /// <param name="epheflag">ephemeris flag</param>
        /// <param name="rsmi">integer specifying that rise, set, orone of the two meridian transits is wanted.</param>
        /// <param name="geopos">array of three doubles containing geograph. long., lat., height of observer</param>
        /// <param name="atpress">atmospheric pressure in mbar/hPa</param>
        /// <param name="attemp">atmospheric temperature in deg. C</param>
        /// <param name="tret">return address (double) for rise time etc.</param>
        /// <param name="serr">return address for error message</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", CharSet = CharSet.Ansi, EntryPoint = "swe_rise_trans")]
        public extern static Int32 swe_rise_trans(double tjd_ut, Int32 ipl, StringBuilder starname, Int32 epheflag, Int32 rsmi, double[] geopos, double atpress, double attemp, ref double tret, StringBuilder serr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tjd_ut">search after this time (UT)</param>
        /// <param name="ipl">planet number, if planet or moon</param>
        /// <param name="starname">star name, if star</param>
        /// <param name="epheflag">ephemeris flag</param>
        /// <param name="rsmi">integer specifying that rise, set, orone of the two meridian transits is wanted.</param>
        /// <param name="geopos">array of three doubles containing geograph. long., lat., height of observer</param>
        /// <param name="atpress">atmospheric pressure in mbar/hPa</param>
        /// <param name="attemp">atmospheric temperature in deg. C</param>
        /// <param name="horhgt">height of local horizon in deg at the point where the body rises or sets</param>
        /// <param name="tret">return address (double) for rise time etc.</param>
        /// <param name="serr">return address for error message</param>
        /// <returns></returns>
        [DllImport("swedll32.dll", CharSet = CharSet.Ansi, EntryPoint = "swe_rise_trans_true_hor")]
        public extern static Int32 swe_rise_trans_true_hor(double tjd_ut, Int32 ipl, StringBuilder starname, Int32 epheflag, Int32 rsmi, double[] geopos, double atpress, double attemp, double horhgt, ref double tret, StringBuilder serr);
    }
}
