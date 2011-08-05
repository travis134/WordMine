namespace WordMine
{
    public enum SpriteStripId
    {
        //Universal
        Idle,           //Prospector: Blinks
                        //MineCart: Does nothing

        //Prospector Specific
        Bored,          //Prospector: Taps and Looks side to side
        Worried,        //Prospector: Waves arms in the air
        Sign,           //Prospector: Shows a blank sign

        //MineCart Specific
        Gold1,
        Gold2,
        Gold3,
        TNT,
        Lightning,
        Explode,        //MineCart: Explodes from TNT
        

    }
}
