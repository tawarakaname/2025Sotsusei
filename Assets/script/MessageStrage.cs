using System.Collections.Generic;

public class MessageStrage
{

    public Dictionary<Item.Type, string> TextDictionary { get; } = new()
    {
        { Item.Type.capsuleA, "H: なんかおちてたよ！\nD: なんだろうそれ！" },
        { Item.Type.capsuleB, "H: これ何かなー？\nD: これは…！" },
        { Item.Type.capsuleC, "H: またあった！\nD: どんどん！" },
        { Item.Type.bluekey, "H: 青いカギだ！！\nD: キミなら必ず出来ると\nD:思っていたさ、ふんふん" },
        { Item.Type.heartcup, "H: コップだね\nD: なにかマークが…" },
        { Item.Type.diacup, "D: なにか見つけたかい？\nH: うん！" },
        { Item.Type.starcup, "D: ふんふん\nH:このマークだいすき！" },
        { Item.Type.heartcup2, "H: 水だー！\nD: のんじゃダメだよ\nD:ふんふん" },
        { Item.Type.diacup2, "D: ん？このにおいは…\nH: すっぱいにおいがする！\nD:ふんふん?" },
        { Item.Type.starcup2, "D: いい調子さ\nD:ふんふーん" },

    };

    // Item.Type に基づくテキスト辞書

    // キーワードに基づくテキスト辞書
    public Dictionary<string, string> KeywordTextDictionary { get; } = new()
    {
        { "smell1", "H: hallo\nD: You smell something strange." },
        { "smell2", "H: すっぱいにおいがする！\nD: そうだね、少し匂におうよね" },
        { "smell3", "H: くさくない\nふんふん\nD: これ何だろう？" },
        { "NoteA", "H:ぼくのノートだ!!!!!\nD: これ、キミのノートだったの？" },
        { "Miss", "H: これじゃないみたい...\nD: 何かちがうみたいだね" },
        { "BalloonStand", "H: 何かはさめそうだね\nD: このマーク…" },
        { "Hint1", "D:おいらはドンドン！\nD: このへやのオトモだよ！\nD: ヒントがほしくなったら\nD: はなしかけてね！\nD: どんどーん！" },
        { "Hint2", "D: このへやくらい！\nD: 火がつけれないかな…。\nD: あっ！\nD:こくばんに何か書いてあるよ！\nD: どんどーん！" },
        { "Hint3", "D: メモがあったんだね！ \nD:メモのマークと同じボタンが \nD:あった気がする！" },
        { "Hint4", "D: その赤い箱のボタン\nD:もしかして火の大きさに\nD:かんけいあるのかな…。\nD:どんどーん！" },
        { "Hint5", "D: さっきからこげくさいな〜？\nD:あっ！！かべこげてるよ！！\nD:あれって、あそこの\nD:たくさんのカードとにてない！？\nD:どんどん！\nH:アルファベットとすうじ…？\nH:すうじだったら\nH:ぼくひろったよ！" },
        { "Hint6", "D: へやの火たち \nD:カラフルになった！\nD:きれい！どんどん…！" },
        { "Hint7", "D: このノート\nD:ふうせんがかいてある！\nD:ふうせんのマーク\nD:どこかで見なかった？\nD:どんどーん！" },
        { "Hint8", "D: カギがあったんだ！！\nD:これで外にでれるね！\nD:どんどん！" },
        { "BlueBox", "D: カギがかかってる！\nH:青いかぎ、もってないね\nD:いまは開けられない…\nD:どんどん…" },
        { "Hint9", "D: 今ね、ここのそうじを\nD:してたんだけど…\nD:中のようすがさっきと\nD:ちがうんだよー！\nD:なにか知ってるー？\nD:どんどん！" },
    };
}