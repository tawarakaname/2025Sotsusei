public static class Textkeyword
{
    public static string GetKeywordBasedOnFlags()
    {
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon1kaiwa))
            return "Hint1";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon1kaiwa) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen))
            return "Hint2";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear))
            return "Hint3";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear))
            return "Hint4";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.capsuleclear))
            return "Hint5";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.capsuleclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear))
            return "Hint6";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget))
            return "Hint7";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
            return "Hint8";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackAanim) &&
            !FlagManager.Instance.GetFlagByType(Item.Type.BTB))
            return "Hint9";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
            FlagManager.Instance.GetFlagByType(Item.Type.BTB))
            return "Hint10";

        return null; // 該当するキーワードがない場合
    }
}
