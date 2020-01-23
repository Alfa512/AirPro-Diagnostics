namespace AirPro.Common.Enumerations
{
    public enum PointOfImacts
    {
        //Migration result
        //FrontLeft = 1,    //1 => 11 (LeftFrontCorner)
        //FrontRight = 2,   //2 => 1 (RightFrontCorner)
        //MiddleLeft = 3,   //3 => 9 (LeftSide)
        //MiddleRight = 4,  //4 => 3 (RightSide)
        //RearLeft = 5,     //5 => 8 (LeftRearSide)
        //RearRight = 6,    //6 => 4 (RightRearSide)

        RightFrontCorner = 1, 
        RightFrontSide = 2, 
        RightSide = 3, 
        RightRearSide = 4, 
        RightRearCorner = 5, 
        RearCenter = 6, 
        LeftRearCorner = 7, 
        LeftRearSide = 8, 
        LeftSide = 9, 
        LeftFrontSide = 10,
        LeftFrontCorner = 11, 
        FrontCenter = 12,
        Rollover = 13 
    }
}
