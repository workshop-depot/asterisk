module Computer

open Sweph
open Sweph.Constants
open Astrobserve
open Factory
open Calculator

let FindAster id_ (time_: JulianDay) pos_ =
    let jd_ = time_

    match id_ with
    | Body p ->
        let pl = ObservePoint p jd_
        T (Body p, AsterBook(Body p, Disposition (pl.Position, pl.Flow)))
    | Angle a ->
        T (Angle a, AsterBook(Angle a, Longitude <| ObserveAscendant pos_ jd_ a))

let For (time: JulianDay) (pos: Position) =
    let ForHelper jd_ id_ = FindAster id_ jd_ pos

    let jd = time
    
    { Asc = ForHelper jd (Angle Bag.SE_ASC);
      Su  = ForHelper jd (Body Su);
      Mo  = ForHelper jd (Body Mo);
      Me  = ForHelper jd (Body Me);
      Ve  = ForHelper jd (Body Ve);
      Ma  = ForHelper jd (Body Ma);
      Ju  = ForHelper jd (Body Ju);
      Sa  = ForHelper jd (Body Sa);
      Ur  = ForHelper jd (Body Ur);
      Ne  = ForHelper jd (Body Ne);
      Pl  = ForHelper jd (Body Pl);
      Ra  = ForHelper jd (Body Ra);
      Ke  = ForHelper jd (Body Ke) }
