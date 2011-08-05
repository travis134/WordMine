using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#if WINDOWS_PHONE
using ShakeGestures;
#endif


namespace WordMine
{
    class PlayScene : InteractableScene
    {

        //Background Positions
        private const int BACKGROUND_OFFSET_X = 400;
        private const int BACKGROUND_OFFSET_Y = 240;

        private const int TRACK_OFFSET_X = 320;
        private const int TRACK_OFFSET_Y = 240;
        private const int TRACK_SPACING_X = 70;

        private const int TRACK_STOP_OFFSET_X = 323;
        private const int TRACK_STOP_OFFSET_Y = 465;
        private const int TRACK_STOP_SPACING_X = 70;

        private const int LEVEL_BAR_OFFSET_X = 235;
        private const int LEVEL_BAR_OFFSET_Y = 280;

        //Menu Positions
        private const int SIDEBAR_OFFSET_X = 105;
        private const int SIDEBAR_OFFSET_Y = 240;

        private const int MINER_OFFSET_X = 105;
        private const int MINER_OFFSET_Y = 360;

        private const int BUTTON_BONUS_OFFSET_X = 95;
        private const int BUTTON_BONUS_OFFSET_Y = 40;

        private const int BUTTON_SCORE_OFFSET_X = 95;
        private const int BUTTON_SCORE_OFFSET_Y = 110;

        private const int BUTTON_WORD_OFFSET_X = 95;
        private const int BUTTON_WORD_OFFSET_Y = 180;

        private const int BUTTON_MENU_OFFSET_X = 95;
        private const int BUTTON_MENU_OFFSET_Y = 250;

        //Play Area Positions
        private const int CART_OFFSET_X = 320;
        private const int CART_OFFSET_Y = 40;
        private const int CART_OFFSET_ODD_Y = 40;
        private const int CART_SPACING_X = 70;
        private const int CART_SPACING_Y = 78;

        //Game Parameters
        private const int CART_MIN_ROWS = 5;
        private const int CART_MAX_ROWS = 6;
        private const int CART_COLUMNS = 7;

        private const int CART_SPEED_START_GAME = 150;
        private const int CART_SPEED_PLAYING_GAME = 5;

        private const int ONE_NUGGET_VALUE = 25;
        private const int TWO_NUGGET_VALUE = 50;
        private const int THREE_NUGGET_VALUE = 100;

        //Scoring
        private int level;
        private int score;
        private int levelScore;
        private int levelMaxScore;
        private String highestPointWord;
        private int highestPointWordScore;
        private String longestWord;
        private Dictionary<String, int> wordsMade;
        private int cartBonus;
        private bonusword bonusWord;
        private Boolean leveledUp;
        private double bonusWordLength;
        //Background GameObjects
        private GameObject background;
        public LevelBar levelBar;
        private List<GameObject> tracks;
        private List<GameObject> trackStops;

        //Menu GameObjects
        private GameObject sideBar;
        private GameObject miner;
        private TokenGameObject buttonMenu;
        private TokenGameObject buttonScore;
        private TokenGameObject buttonBonus;
        private TokenGameObject buttonWord;

        //Play Area GameObjects
        public MineCart[,] mineCarts;
        private List<MineCart> mineCartsTNT;
        private List<AnimatedInteractableGameObject> explosions;
        private List<MineCart> lastMineCarts;

        private PopupScoreScene popupScoreScene;
        private PopupMenuScene popupMenuScene;
        private PopupLoseScene popupLoseScene;
        private PopupBonusScene popupBonusScene;
        private PopupQuizScene popupQuizScene;
        
        //Game Data
        private Random rand;

        private String language;

        private Dictionary<String, Double> alphabet;
        private List<String> dictionary;
        private List<bonusword> bonusEnglish;
        private List<String> bonus;
        private List<char> bonusSeed;

        private Boolean isBonus;
        private Boolean startGame;
        private int startCurrentX = 0;
        private int startCurrentY = 0;
        private String builtString;
        private String bonusString;

        private float tntChance = 0.2f;
        private int numberOfTNT = 0;
        private int maxNumberOfTNT = 0;

        private Boolean animating;
        private Boolean usedTNT;


        public PlayScene()
            : base()
        {
#if WINDOWS_PHONE
            ShakeGesturesHelper.Instance.ShakeGesture += new    
            EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture); 
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 4;
            ShakeGesturesHelper.Instance.Active = true;
#endif
            this.sceneIndex = SceneIndex.PlayScene;
        }

#if WINDOWS_PHONE

        private void Instance_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {
            randomizeCarts();
        }
#endif

        public override void Stop()
        {
            base.Stop();
            //Background GameObjects
            background = null;
            levelBar = null;
            tracks = null;
            trackStops = null;

            //Menu GameObjects
            sideBar = null;
            miner = null;
            buttonMenu = null;
            buttonScore = null;
            buttonBonus = null;
            buttonWord = null;
            bonusEnglish = null;
            //Play Area GameObjects
            mineCarts = null;
            mineCartsTNT = null;
            explosions = null;
            lastMineCarts = null;
            popupScoreScene = null;
            popupMenuScene = null;
            popupLoseScene = null;
            popupBonusScene = null;
            popupQuizScene = null;
        }

        public override void Start()
        {
            base.Start();

            level = 1;
            startGame = true;
            startCurrentX = 0;
            startCurrentY = 0;
            score = 000;
            levelScore = 000;
            levelMaxScore = 1000;
            builtString = "";
            this.language = "english";
            lose = false;
bonusEnglish=new List<bonusword>();
            isBonus = false;
            tntChance = 0.2f;
            numberOfTNT = 0;
            maxNumberOfTNT = 0;

            animating = false;
            usedTNT = false;

            this.dictionary = null;
            this.alphabet = null;
            this.bonus = null;
            this.bonusSeed = null;
            this.bonusWordLength = 3;

            //Game Data Initialization
            rand = new Random();
            alphabet = new Dictionary<String, Double>();
            dictionary = new List<String>();
            bonus = new List<String>();
            bonusSeed = new List<char>();
            startGame = true;
            highestPointWord="";
            highestPointWordScore=0;
            longestWord="";

            popupScoreScene = new PopupScoreScene();
            popupMenuScene = new PopupMenuScene();
            popupLoseScene = new PopupLoseScene();
            popupBonusScene = new PopupBonusScene();
            popupQuizScene = new PopupQuizScene();

            wordsMade = new Dictionary<String, int>();
            //Background Initialization
            background = new GameObject("background/playScene2", new Vector2(BACKGROUND_OFFSET_X, BACKGROUND_OFFSET_Y));
            background.zindex = 0.1f;
            gameObjects.Add(background);
            cartBonus = 1;
            tracks = new List<GameObject>();
            trackStops = new List<GameObject>();
            GameObject temp;
            for (int i = 0; i < CART_COLUMNS; i++)
            {
                temp = new GameObject("background/tracks/track" + (i + 1).ToString(), new Vector2(TRACK_OFFSET_X + (i * TRACK_SPACING_X), TRACK_OFFSET_Y));
                temp.zindex = 0.2f;
                tracks.Add(temp);
                gameObjects.Add(temp);

                if ((i % 2) == 0)
                {
                    temp = new GameObject("foreground/trackStop", new Vector2(TRACK_STOP_OFFSET_X + (i * TRACK_STOP_SPACING_X), TRACK_STOP_OFFSET_Y));
                    temp.zindex = 0.21f;
                    trackStops.Add(temp);
                    gameObjects.Add(temp);
                }
            }
            leveledUp = false;
            levelBar = new LevelBar("menu/levelBarBackground", new Vector2(LEVEL_BAR_OFFSET_X, LEVEL_BAR_OFFSET_Y), "menu/levelBarTop", "menu/levelBarFill");
            levelBar.noAnimate = true;
            levelBar.zindex = 0.2f;
            gameObjects.Add(levelBar);

            //Menu Initialization

            sideBar = new GameObject("menu/sidebar", new Vector2(SIDEBAR_OFFSET_X, SIDEBAR_OFFSET_Y));
            sideBar.zindex = 0.2f;
            gameObjects.Add(sideBar);

            miner = new GameObject("menu/miner", new Vector2(MINER_OFFSET_X, MINER_OFFSET_Y));
            miner.zindex = 0.3f;
            gameObjects.Add(miner);

            buttonBonus = new TokenGameObject("menu/buttonJumble", new Vector2(BUTTON_BONUS_OFFSET_X, BUTTON_BONUS_OFFSET_Y), "");
            buttonScore = new TokenGameObject("menu/buttonScore", new Vector2(BUTTON_SCORE_OFFSET_X, BUTTON_SCORE_OFFSET_Y), "0");
            buttonWord = new TokenGameObject("menu/buttonWord", new Vector2(BUTTON_WORD_OFFSET_X, BUTTON_WORD_OFFSET_Y), "");
            buttonMenu = new TokenGameObject("menu/buttonMenu", new Vector2(BUTTON_MENU_OFFSET_X, BUTTON_MENU_OFFSET_Y), "Menu");

            buttonBonus.zindex = 0.3f;
            buttonScore.zindex = 0.3f;
            buttonWord.zindex = 0.3f;
            buttonMenu.zindex = 0.3f;

            buttonScore.fontColor = Color.Gold;

            buttonBonus.fontRotation = -0.1f;
            buttonWord.fontRotation = 0.1f;

            buttonBonus.noAnimate = true;
            buttonScore.noAnimate = true;
            buttonWord.noAnimate = true;
            buttonMenu.noAnimate = true;

            buttonBonus.fontPath = "fonts/WesternSmall";
            buttonScore.fontPath = "fonts/WesternSmall";
            buttonWord.fontPath = "fonts/WesternSmall";
            buttonMenu.fontPath = "fonts/WesternSmall";

            gameObjects.Add(buttonBonus);
            gameObjects.Add(buttonScore);
            gameObjects.Add(buttonWord);
            gameObjects.Add(buttonMenu);

            //Play Area Initialization

            mineCarts = new MineCart[CART_COLUMNS, CART_MAX_ROWS];

            lastMineCarts = new List<MineCart>();

            mineCartsTNT = new List<MineCart>();

            AnimatedInteractableGameObject explosion;

            explosions = new List<AnimatedInteractableGameObject>();
            for (int i = 0; i < 5; i++)
            {
                explosion = new AnimatedInteractableGameObject("foreground/explosion", new Vector2(0, 0), 25, 5, 10);
                explosion.zindex = 0.4f;
                explosion.finished = true;
                gameObjects.Add(explosion);
                explosions.Add(explosion);
            }
        }

        public override void LoadContent(ContentManager content)
        {
            if (this.options != null)
            {
                this.options.TryGetValue("language", out this.language);
            }

            try
            {
                this.alphabet = content.Load<Dictionary<String, Double>>("data/alphabet_" + this.language);
                this.dictionary = content.Load<List<String>>("data/dictionary_" + this.language);
                if (this.language == "english")
                {
                    bonusEnglish.Add(new bonusword(
                    "adjective"
                    , "able"
                    , "capable"
                    , "capable"
                ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "agile"
                        , "active"
                        , "nimble, spry, quick"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "aloof"
                        , "reserved"
                        , "distant"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "amber"
                        , "chromatic"
                        , "yellowish"
                    ));

                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "ample"
                        , "large"
                        , "sizable, capacious"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "arrogant"
                        , "proud"
                        , "self-important"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "astonishing"
                        , "impressive"
                        , "astounding, staggering"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bad"
                        , "evil"
                        , "immoral, evil"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bad"
                        , "inferior"
                        , "below average in quality"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bad"
                        , "stale"
                        , "spoiled, spoilt, capable of harming"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bald"
                        , "hairless"
                        , "lacking hair"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bitter"
                        , "resentful"
                        , "acrimonious, resentful"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bitter"
                        , "tasty"
                        , "bitter-tasting"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "black"
                        , "undiluted"
                        , "without cream or sugar"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "bland"
                        , "tasteless"
                        , "tasteless, insipid, flavorless"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "blank"
                        , "empty"
                        , "empty, not filled in"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "brown"
                        , "chromatic"
                        , "having a brown color"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "charismatic"
                        , "attractive"
                        , "possessing a magnetic personality"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "childish"
                        , "immature"
                        , "infantile"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "common"
                        , "shared"
                        , "mutual"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "constant"
                        , "continuous"
                        , "unending, incessant"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "critical"
                        , "indispensable"
                        , "vital urgently needed"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "crude"
                        , "early"
                        , "primitive"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "cruel"
                        , "inhumane"
                        , "brutal, barbarous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "cute"
                        , "attractive"
                        , "attractive"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "deadly"
                        , "fatal"
                        , "lethal"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "decorative"
                        , "nonfunctional"
                        , "cosmetic, ornamental"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "delicate"
                        , "breakable"
                        , "fragile, frail, easily broken, sensitive"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "articulate"
                        , "articulate"
                        , "eloquent, well-spoken"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "eternal"
                        , "permanent"
                        , "everlasting, perpetual, unending"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "ethnic"
                        , "social"
                        , "cultural"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "exotic"
                        , "foreign"
                        , "foreign, alien"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "exotic"
                        , "strange"
                        , "unusual, strikingly strange"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "express"
                        , "fast"
                        , "without unnecessary stops"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "finished"
                        , "destroyed"
                        , "ruined"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "flat"
                        , "horizontal"
                        , "horizontally level"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "flawed"
                        , "imperfect"
                        , "imperfect, blemished, faulty"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "frank"
                        , "direct"
                        , "candid, blunt, forthright"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "free"
                        , "unpaid"
                        , "complimentary, costless, gratis"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "free"
                        , "unoccupied"
                        , "not occupied"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "fresh"
                        , "forward"
                        , "insolent, impertinent, impudent, sassy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "frozen"
                        , "unmelted"
                        , "unthawed"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "full"
                        , "nourished"
                        , "replete, filled to satisfaction with food"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "funny"
                        , "humorous"
                        , "amusing, laughable"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "good"
                        , "healthful"
                        , "beneficial, salutary"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "good"
                        , "righteous"
                        , "just, upright, virtuous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "grand"
                        , "rich"
                        , "luxurious, opulent, sumptuous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "graphic"
                        , "explicit"
                        , "explicit, descriptive"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "graphic"
                        , "realistic"
                        , "pictorial, lifelike, vivid"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "great"
                        , "important"
                        , "outstanding, very valuable"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "great"
                        , "large"
                        , "large in size, number or extent"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "handy"
                        , "convenient"
                        , "easy to use"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "helpless"
                        , "powerless"
                        , "incapacitated"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "hilarious"
                        , "humorous"
                        , "uproarious"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "huge"
                        , "large"
                        , "immense, vast"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "ignorant"
                        , "uneducated"
                        , "lacking basic knowledge, naive, unsophisticated"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "immune"
                        , "unsusceptible"
                        , "resistant"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "incapable"
                        , "inadequate"
                        , "incompetent"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "jealous"
                        , "desirous"
                        , "covetous, envious"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "last"
                        , "closing"
                        , "concluding, final, terminal"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "late"
                        , "unpunctual"
                        , "belated, tardy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "latest"
                        , "fashionable"
                        , "newest, up-to-date"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "lazy"
                        , "idle"
                        , "indolent, otiose, slothful, work-shy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "lonely"
                        , "unaccompanied"
                        , "alone, lone, solitary"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "depressed"
                        , "dejected"
                        , "blue"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "main"
                        , "important"
                        , "chief, primary, principal"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "miserable"
                        , "contemptible"
                        , "abject, scummy, contemptible"
                    ));

                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "monstrous"
                        , "evil"
                        , "atrocious, heinous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "monstrous"
                        , "ugly"
                        , "grotesque"
                    ));

                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "nervous"
                        , "excitable"
                        , "skittish"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "new"
                        , "unaccustomed"
                        , "unfamiliar"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "notorious"
                        , "disreputable"
                        , "ill-famed, infamous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "obese"
                        , "fat"
                        , "overweight"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "obscure"
                        , "inglorious"
                        , "unknown"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "paralyzed"
                        , "ill"
                        , "paralytic, unable to move"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "particular"
                        , "fastidious"
                        , "finicky, fussy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "particular"
                        , "specific"
                        , "peculiar, special"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "premature"
                        , "early"
                        , "untimely"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "private"
                        , "personal"
                        , "concerning things personal"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "rare"
                        , "infrequent"
                        , "infrequent, uncommon"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "real"
                        , "true"
                        , "actual, genuine"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "reckless"
                        , "bold"
                        , "foolhardy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "restless"
                        , "unquiet"
                        , "antsy, itchy, fidgety"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "retired"
                        , "inactive"
                        , "no longer active in your work"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "romantic"
                        , "loving"
                        , "amatory, amorous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "rotten"
                        , "unsound"
                        , "decayed, rotted"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "rotten"
                        , "bad"
                        , "crappy, lousy, shitty, stinking, stinky"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "satisfied"
                        , "mitigated"
                        , "quenched, slaked"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "secular"
                        , "profane"
                        , "laic, lay"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "shallow"
                        , "superficial"
                        , "lacking depth of intellect or knowledge or feeling"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "shy"
                        , "unconfident"
                        , "timid, diffident"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "single"
                        , "unshared"
                        , "individual, separate"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "sticky"
                        , "adhesive"
                        , "gluey, glutinous, gummy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "muggy"
                        , "wet"
                        , "sticky, steamy"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "still"
                        , "nonmoving"
                        , "inactive, motionless, static"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "strong"
                        , "alcoholic"
                        , "hard, having a high alcoholic content"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "strong"
                        , "forceful"
                        , "firm"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "strong"
                        , "invulnerable"
                        , "secure, unattackable"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "stunning"
                        , "beautiful"
                        , "strikingly beautiful or attractive"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "supplementary"
                        , "secondary"
                        , "auxiliary, subsidiary"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "talkative"
                        , "voluble"
                        , "chatty, gabby, garrulous"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "terminal"
                        , "last"
                        , "endmost"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "thoughtful"
                        , "considerate"
                        , "considerate, showing concern"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "transparent"
                        , "thin"
                        , "see-through, sheer"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "trivial"
                        , "ordinary"
                        , "banal, commonplace"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "unanimous"
                        , "accordant"
                        , "in complete agreement"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "unique"
                        , "incomparable"
                        , "unequaled, unparalleled"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "global"
                        , "comprehensive"
                        , "universal, worldwide"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "unlawful"
                        , "illegal"
                        , "illegitimate, illicit"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "vain"
                        , "proud"
                        , "self-conceited, swollen-headed"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "viable"
                        , "possible"
                        , "feasible, practicable, workable"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "vigorous"
                        , "robust"
                        , "strong physically or mentally"
                    ));
                    bonusEnglish.Add(new bonusword(
                        "adjective"
                        , "young"
                        , "young"
                        , "youthful"
                    ));

                }
                else
                    this.bonus = content.Load<List<String>>("data/bonus_" + this.language);
            }
            catch (ContentLoadException e)
            {
                Console.Out.WriteLine("Error: " + e.Message + "\nDefaulting to english");
                this.alphabet = content.Load<Dictionary<String, Double>>("data/alphabet_english");
                this.dictionary = content.Load<List<String>>("data/dictionary_english");
                this.bonusEnglish = content.Load<List<bonusword>>("data/bonus_english");
            }

            newBonusWord();

            this.popupScoreScene.LoadContent(content);
            this.popupMenuScene.LoadContent(content);
            this.popupLoseScene.LoadContent(content);
            this.popupBonusScene.LoadContent(content);
            this.popupQuizScene.LoadContent(content);

            for (int i = 0; i < CART_COLUMNS; i++)
            {
                for (int j = 0; j < CART_MAX_ROWS; j++)
                {
                    if (((i % 2) == 0) && (j >= CART_MIN_ROWS))
                    {
                        mineCarts[i, j] = null;
                    }
                    else
                    {
                        mineCarts[i, j] = new MineCart();
                    }

                    if (mineCarts[i, j] != null)
                    {
                        mineCarts[i, j].x = i;
                        mineCarts[i, j].y = j;

                        mineCarts[i, j].position.X = CART_OFFSET_X + (i * CART_SPACING_X);
                        mineCarts[i, j].position.Y = -CART_OFFSET_Y;

                        mineCarts[i, j].contents = randomLetter().ToUpper();

                        if ((alphabet[mineCarts[i, j].contents.ToLower()] >= 0f / 3f) && (alphabet[mineCarts[i, j].contents.ToLower()] < 1f / 3f))
                        {
                            mineCarts[i, j].mineCartType = MineCartType.Gold1;
                        }
                        else if ((alphabet[mineCarts[i, j].contents.ToLower()] >= 1f / 3f) && (alphabet[mineCarts[i, j].contents.ToLower()] < 2f / 3f))
                        {
                            mineCarts[i, j].mineCartType = MineCartType.Gold2;
                        }
                        else if (alphabet[mineCarts[i, j].contents.ToLower()] >= 2f / 3f)
                        {
                            mineCarts[i, j].mineCartType = MineCartType.Gold3;
                        }

                        mineCarts[i, j].zindex = 0.3f;

                        gameObjects.Add(mineCarts[i, j]);
                    }
                }
            }
            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
#if WINDOWS_PHONE
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.control = SceneControls.Goto;
                this.sceneIndex = SceneIndex.MenuScene;
                this.end = true;
            }
#endif
            if (this.language == "english")
            {
                if (this.popupBonusScene.quiz)
                {
                    this.popupQuizScene.dismissed = false;
                    this.popupBonusScene.quiz = false;
                }
            }
            if (!this.popupScoreScene.dismissed)
            {
                this.popupScoreScene.Update(gameTime);
            }
            else if (!this.popupQuizScene.dismissed)
            {
                this.popupQuizScene.Update(gameTime);
            }
            else if (!this.popupBonusScene.dismissed)
            {
                this.popupBonusScene.Update(gameTime);
            }
            else if (!this.popupMenuScene.dismissed)
            {
                this.popupMenuScene.Update(gameTime);
                if (this.popupMenuScene.exit)
                {
                    this.control = SceneControls.Goto;
                    this.sceneIndex = SceneIndex.MenuScene;
                    this.end = true;
                }
                else if (this.popupMenuScene.newGame)
                {
                    this.control = SceneControls.Reset;
                    this.end = true;
                }
            }
            else if (!this.popupLoseScene.dismissed)
            {
                this.popupLoseScene.Update(gameTime);
                if (this.popupLoseScene.newGame)
                {
                    this.control = SceneControls.Reset;
                    this.end = true;
                }
            }
            else
            {

                if (clicked == buttonMenu)
                {
                    this.popupMenuScene.dismissed = false;
                }
                if (startGame)
                {

                    MineCart temp = mineCarts[startCurrentX, startCurrentY];

                    if (temp != null)
                    {
                        int mineCartStopPositionY = (Game1.CAMERA_HEIGHT - (temp.y * CART_SPACING_Y)) - CART_OFFSET_Y;
                        if ((temp.x % 2) == 0)
                        {
                            mineCartStopPositionY -= CART_OFFSET_ODD_Y;
                        }

                        if ((temp.position.Y + CART_SPEED_START_GAME) <= mineCartStopPositionY)
                        {
                            temp.position.Y += CART_SPEED_START_GAME;
                        }
                        else
                        {
                            temp.finished = false;
                            temp.position.Y = mineCartStopPositionY;
                            if (startCurrentY < CART_MAX_ROWS - 1)
                            {
                                startCurrentY++;
                            }
                            else
                            {
                                if (startCurrentX < CART_COLUMNS - 1)
                                {
                                    startCurrentX++;
                                    startCurrentY = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (startCurrentX < CART_COLUMNS - 1)
                        {
                            startCurrentX++;
                            startCurrentY = 0;
                        }
                        else
                        {
                            startGame = false;
                        }
                    }
                }
                else
                {


                    //Check Word
                    if (cursor.stoppedDragging && !leveledUp)
                    {
                        if (builtString != null)
                        {
                            if (dictionary.Contains(builtString.ToLower()))
                            {
                                if (builtString.ToUpper() == bonusString)
                                {
                                    isBonus = true;
                                    bonusWordLength += .2;
                                }



                                for (int i = 0; i < lastMineCarts.Count; i++)
                                {
                                    if (lastMineCarts[i].mineCartType == MineCartType.TNT)
                                    {
                                        usedTNT = true;
                                        explosions[mineCartsTNT.IndexOf(lastMineCarts[i])].position = lastMineCarts[i].position;
                                        explosions[mineCartsTNT.IndexOf(lastMineCarts[i])].Begin();
                                        mineCartsTNT.Remove(lastMineCarts[i]);
                                        numberOfTNT--;
                                        foreach (MineCart adjacent in getAdjacentMineCarts(lastMineCarts[i]))
                                        {
                                            if (!lastMineCarts.Contains(adjacent) && adjacent.mineCartType != MineCartType.TNT)
                                            {
                                                lastMineCarts.Add(adjacent);
                                            }
                                        }
                                    }
                                    if (lastMineCarts[i].bonus == true)
                                    {
                                        cartBonus++;
                                        lastMineCarts[i].tintColor = Color.White;
                                        lastMineCarts[i].bonus = false;
                                    }
                                }
                                calculateScore();

                                if (!usedTNT)
                                {
                                    for (int i = 0; i < mineCartsTNT.Count; i++)
                                    {
                                        if (mineCartsTNT[i].y > 0)
                                        {
                                            if (mineCarts[mineCartsTNT[i].x, mineCartsTNT[i].y - 1].mineCartType != MineCartType.TNT)
                                            {
                                                lastMineCarts.Add(mineCarts[mineCartsTNT[i].x, mineCartsTNT[i].y - 1]);
                                            }
                                        }
                                    }
                                }

                                lastMineCarts.Sort();
                                foreach (MineCart temp in lastMineCarts)
                                {
                                    removeMineCart(temp, usedTNT);
                                }
                                int switchTNT = 0;
                                bool makeBonus = false;
                                switch (builtString.Length)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                        break;
                                    case 4:
                                        if (rand.NextDouble() > .5)
                                            makeBonus = true;
                                        break;
                                    case 5:
                                        makeBonus = true;
                                        if (rand.NextDouble() > .5 && mineCartsTNT.Count > 0)
                                        {
                                            switchTNT = 1;
                                        }
                                        break;
                                    case 6:
                                        makeBonus = true;
                                        if (mineCartsTNT.Count > 0)
                                        {
                                            switchTNT = 1;
                                        }
                                        break;
                                    default:
                                        makeBonus = true;
                                        switchTNT = mineCartsTNT.Count;
                                        break;
                                }
                                if (makeBonus)
                                {
                                    MineCart mineBonus = null;
                                    bool leaveLoop = false;
                                    while (!leaveLoop)
                                    {
                                        mineBonus = mineCarts[rand.Next(0, CART_COLUMNS - 1), rand.Next(0, CART_MAX_ROWS - 1)];
                                        if (mineBonus != null && mineBonus.mineCartType != MineCartType.TNT)
                                            leaveLoop = true;
                                    }
                                    mineBonus.bonus = true;
                                    mineBonus.tintColor = Color.LawnGreen;
                                }
                                for (int tnt = switchTNT - 1; tnt >= 0; tnt--)
                                {
                                    if ((alphabet[mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].contents.ToLower()] >= 0f / 3f) && (alphabet[mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].contents.ToLower()] < 1f / 3f))
                                    {
                                        mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].mineCartType = MineCartType.Gold1;
                                    }
                                    else if ((alphabet[mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].contents.ToLower()] >= 1f / 3f) && (alphabet[mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].contents.ToLower()] < 2f / 3f))
                                    {
                                        mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].mineCartType = MineCartType.Gold2;
                                    }
                                    else if (alphabet[mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].contents.ToLower()] >= 2f / 3f)
                                    {
                                        mineCarts[mineCartsTNT[tnt].x, mineCartsTNT[tnt].y].mineCartType = MineCartType.Gold3;
                                    }
                                    mineCartsTNT.Remove(mineCartsTNT[tnt]);
                                    numberOfTNT--;
                                }
                            }
                            else
                            {
                                buttonWord.fontColor = Color.Red;
                            }
                        }
                        foreach (MineCart temp in lastMineCarts)
                        {
                            temp.fontColor = Color.White;
                        }
                        usedTNT = false;
                        builtString = null;
                        lastMineCarts.Clear();
                    }
                    if (!this.animating)
                    {
                        //Lose Game

                        if (!this.lose)
                        {
                            //Selecting Word
                            if (cursor.dragging)
                            {
                                for (int i = 0; i < CART_COLUMNS; i++)
                                {
                                    for (int j = 0; j < CART_MAX_ROWS; j++)
                                    {
                                        if (mineCarts[i, j] != null)
                                        {
                                            if (cursorRectangle.Intersects(mineCarts[i, j].rectangle))
                                            {
                                                buttonWord.fontColor = Color.White;
                                                if ((lastMineCarts.Count < 1) || (isMineCartAdjacent(lastMineCarts[lastMineCarts.Count - 1], mineCarts[i, j])))
                                                {
                                                    if (!lastMineCarts.Contains(mineCarts[i, j]))
                                                    {

                                                        mineCarts[i, j].finished = false;
                                                        foreach (MineCart m in getAdjacentMineCarts(mineCarts[i, j]))
                                                        {
                                                            if (m.fontColor != Color.Gold)
                                                            {
                                                                m.finished = false;
                                                            }
                                                        }
                                                        mineCarts[i, j].fontColor = Color.Gold;
                                                        lastMineCarts.Add(mineCarts[i, j]);
                                                        builtString += mineCarts[i, j].contents.ToLower();
                                                        buttonWord.contents = builtString.ToUpper();

                                                    }
                                                    else if (lastMineCarts.Count > 1)
                                                    {
                                                        //ArgumentOutOfRange Exception
                                                        if (mineCarts[i, j] == lastMineCarts[lastMineCarts.Count - 2])
                                                        {
                                                            lastMineCarts[lastMineCarts.Count - 1].fontColor = Color.White;
                                                            builtString = builtString.Substring(0, builtString.Length - 1);
                                                            buttonWord.contents = builtString;
                                                            lastMineCarts.Remove(lastMineCarts[lastMineCarts.Count - 1]);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    this.animating = false;

                    //Reposition carts
                    for (int i = 0; i < CART_COLUMNS; i++)
                    {
                        for (int j = 0; j < CART_MAX_ROWS; j++)
                        {
                            if (mineCarts[i, j] != null)
                            {
                                int mineCartStopPositionY = (Game1.CAMERA_HEIGHT - (mineCarts[i, j].y * CART_SPACING_Y)) - CART_OFFSET_Y;

                                if ((mineCarts[i, j].x % 2) == 0)
                                {
                                    mineCartStopPositionY -= CART_OFFSET_ODD_Y;
                                }

                                if ((mineCarts[i, j].position.Y + CART_SPEED_PLAYING_GAME) <= mineCartStopPositionY)
                                {
                                    this.animating = true;
                                    mineCarts[i, j].position.Y += CART_SPEED_PLAYING_GAME;
                                }
                                else
                                {
                                    mineCarts[i, j].position.Y = mineCartStopPositionY;
                                }

                            }
                        }
                    }

                    if (!this.animating)
                    {
                        for (int i = 0; i < CART_COLUMNS; i++)
                        {
                            if (mineCarts[i, 0] != null)
                            {
                                if (mineCarts[i, 0].mineCartType == MineCartType.TNT)
                                {
                                    var sortedDict = (from entry in wordsMade orderby entry.Value descending select entry);
                                    this.popupLoseScene.fillIn(longestWord, highestPointWord, highestPointWordScore, sortedDict.ElementAt(0).Key, sortedDict.ElementAt(0).Value, wordsMade.Count);
                                    this.popupLoseScene.dismissed = false;
                                    this.lose = true;
                                }
                            }
                        }
                    }
                }

                if (leveledUp && !lose)
                {
                    if (!levelBar.animating)
                    {
                        newBonusWord();
                        if (maxNumberOfTNT < 5)
                        {
                            maxNumberOfTNT++;
                        }

                        var sortedDict = (from entry in wordsMade orderby entry.Value descending select entry);

                        this.popupScoreScene.fillIn(longestWord, highestPointWord, highestPointWordScore, sortedDict.ElementAt(0).Key, sortedDict.ElementAt(0).Value, wordsMade.Count);
                        this.popupScoreScene.dismissed = false;

                        leveledUp = false;
                        //Code to popup a window displaying these stats needs to trigger here
                        /*
                         * Longest Word: %longestWord
                         * Highest Point Word: %highestPointWord %highestPointWordScore
                         * Most Created Word: %sortedDict.ElementAt(0).Value %sortedDict.ElementAt(0).Key
                         * Total Words Created: %wordsMade.Count
                         * 
                         */
                    }
                }

                if (levelScore >= levelMaxScore && !lose)
                {
                    level++;
                    levelScore -= levelMaxScore;
                    levelMaxScore = (int)(levelMaxScore * 1.35);
                    Console.Out.WriteLine(levelScore + " : " + levelMaxScore);
                    levelBar.levelUp(level);
                    leveledUp = true;
                }

                levelBar.barFillPercent = ((float)levelScore / (float)levelMaxScore);



                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!this.popupScoreScene.dismissed)
            {
                this.popupScoreScene.Draw(spriteBatch);
            }
            else if (!this.popupQuizScene.dismissed)
            {
                this.popupQuizScene.Draw(spriteBatch);
            }
            else if (!this.popupBonusScene.dismissed)
            {
                this.popupBonusScene.Draw(spriteBatch);
            }
            else if (!this.popupMenuScene.dismissed)
            {
                this.popupMenuScene.Draw(spriteBatch);
            }
            else if (!this.popupLoseScene.dismissed)
            {
                this.popupLoseScene.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }

        private void newBonusWord()
        {
            if (this.language == "english")
            {
                bonusWord = this.bonusEnglish[rand.Next(this.bonusEnglish.Count)];
                while (bonusWord.length != (int)Math.Floor(bonusWordLength))
                {
                    bonusWord = this.bonusEnglish[rand.Next(this.bonusEnglish.Count)];
                }
                bonusString = bonusWord.word.ToUpper();
            }
            else
            {
                bonusString = this.bonus[rand.Next(this.bonus.Count)].ToUpper();
                while (bonusString.Length != (int)Math.Floor(bonusWordLength))
                {
                    bonusString = this.bonus[rand.Next(this.bonus.Count)].ToUpper();
                }
            }
            buttonBonus.contents = bonusString;
            
            bonusSeed.Clear();
            foreach (char c in bonusString)
            {
                bonusSeed.Add(c);
            }
            Console.Out.WriteLine(bonusSeed);
        }

        private void calculateScore()
        {
            int tempScore = 0;
            for (int i = 0; i < builtString.Length; i++)
            {
                if ((alphabet[builtString[i].ToString().ToLower()] >= 0f / 3f) && (alphabet[builtString[i].ToString().ToLower()] < 1f / 3f))
                {
                    tempScore += ONE_NUGGET_VALUE;
                }
                else if ((alphabet[builtString[i].ToString().ToLower()] >= 1f / 3f) && (alphabet[builtString[i].ToString().ToLower()] < 2f / 3f))
                {
                    tempScore += TWO_NUGGET_VALUE;
                }
                else if (alphabet[builtString[i].ToString().ToLower()] <= 2f / 3f)
                {
                    tempScore += THREE_NUGGET_VALUE;
                }
            }
            tempScore *= builtString.Length;
            if (isBonus)
            {
                tempScore *= 10;
                isBonus = false;
                if (language == "english")
                {
                    popupBonusScene.fillIn(bonusWord.word, bonusWord.partOfSpeech, bonusWord.similarTo, bonusWord.definition);
                    popupBonusScene.dismissed = false;
                    this.popupQuizScene.fillIn(bonusWord);
                    //PopUpBonusScene; //here
                }

                
                newBonusWord();
            }

            score += tempScore*cartBonus;
            levelScore += tempScore * cartBonus;
            levelBar.barFillPercent = ((float)levelScore / (float)levelMaxScore);


            buttonScore.contents = score.ToString();
            buttonWord.fontColor = Color.Green;
            cartBonus = 1;

            if (builtString.Length > longestWord.Length)
            {
                longestWord = builtString;
            }
            if (highestPointWordScore < tempScore)
            {
                highestPointWordScore = tempScore;
                highestPointWord = builtString;
            }
            if (wordsMade.ContainsKey(builtString))
            {
                wordsMade[builtString]++;
            }
            else
            {
                wordsMade.Add(builtString,1);
            }
        }

        private Boolean isMineCartAdjacent(MineCart lastMineCart, MineCart currentMineCart)
        {
            Boolean flag = false;

            if (currentMineCart.x == lastMineCart.x)
            {
                if ((currentMineCart.y == lastMineCart.y - 1) || (currentMineCart.y == lastMineCart.y + 1))
                {
                    flag = true;
                }
            }
            else if (currentMineCart.x == lastMineCart.x - 1)
            {
                if ((currentMineCart.x % 2) == 0)
                {
                    if ((currentMineCart.y == lastMineCart.y) || (currentMineCart.y == lastMineCart.y - 1))
                    {
                        flag = true;
                    }
                }
                else
                {
                    if ((currentMineCart.y == lastMineCart.y) || (currentMineCart.y == lastMineCart.y + 1))
                    {
                        flag = true;
                    }
                }
            }
            else if (currentMineCart.x == lastMineCart.x + 1)
            {
                if ((currentMineCart.x % 2) == 0)
                {
                    if ((currentMineCart.y == lastMineCart.y) || (currentMineCart.y == lastMineCart.y - 1))
                    {
                        flag = true;
                    }
                }
                else
                {
                    if ((currentMineCart.y == lastMineCart.y) || (currentMineCart.y == lastMineCart.y + 1))
                    {
                        flag = true;
                    }
                }
            }

            return flag;
        }

        private List<MineCart> getAdjacentMineCarts(MineCart currentMineCart)
        {
            List<MineCart> adjacentMineCarts = new List<MineCart>();

            if (currentMineCart.y > 0)
            {
                if (mineCarts[currentMineCart.x, currentMineCart.y - 1] != null)
                {
                    adjacentMineCarts.Add(mineCarts[currentMineCart.x, currentMineCart.y - 1]);
                }
            }


            if ((currentMineCart.x) % 2 == 0)
            {
                if (currentMineCart.y < CART_MIN_ROWS - 1)
                {
                    if (mineCarts[currentMineCart.x, currentMineCart.y + 1] != null)
                    {
                        adjacentMineCarts.Add(mineCarts[currentMineCart.x, currentMineCart.y + 1]);
                    }
                }

                if (currentMineCart.x > 0)
                {
                    if (mineCarts[currentMineCart.x - 1, currentMineCart.y] != null)
                    {
                        adjacentMineCarts.Add(mineCarts[currentMineCart.x - 1, currentMineCart.y]);
                    }
                    if (currentMineCart.y < CART_MAX_ROWS - 1)
                    {
                        if (mineCarts[currentMineCart.x - 1, currentMineCart.y + 1] != null)
                        {
                            adjacentMineCarts.Add(mineCarts[currentMineCart.x - 1, currentMineCart.y + 1]);
                        }
                    }
                }

                if (currentMineCart.x < CART_COLUMNS - 1)
                {
                    if (mineCarts[currentMineCart.x + 1, currentMineCart.y] != null)
                    {
                        adjacentMineCarts.Add(mineCarts[currentMineCart.x + 1, currentMineCart.y]);
                    }
                    if (currentMineCart.y < CART_MAX_ROWS - 1)
                    {
                        if (mineCarts[currentMineCart.x + 1, currentMineCart.y + 1] != null)
                        {
                            adjacentMineCarts.Add(mineCarts[currentMineCart.x + 1, currentMineCart.y + 1]);
                        }
                    }
                }
            }
            else
            {
                if (currentMineCart.y < CART_MAX_ROWS - 1)
                {
                    if (mineCarts[currentMineCart.x, currentMineCart.y + 1] != null)
                    {
                        adjacentMineCarts.Add(mineCarts[currentMineCart.x, currentMineCart.y + 1]);
                    }
                }

                if (currentMineCart.x > 0)
                {
                    if (mineCarts[currentMineCart.x - 1, currentMineCart.y] != null)
                    {
                        adjacentMineCarts.Add(mineCarts[currentMineCart.x - 1, currentMineCart.y]);
                    }
                    if (currentMineCart.y > 0)
                    {
                        if (mineCarts[currentMineCart.x - 1, currentMineCart.y - 1] != null)
                        {
                            adjacentMineCarts.Add(mineCarts[currentMineCart.x - 1, currentMineCart.y - 1]);
                        }
                    }
                }

                if (currentMineCart.x < CART_COLUMNS - 1)
                {
                    if (mineCarts[currentMineCart.x + 1, currentMineCart.y] != null)
                    {
                        adjacentMineCarts.Add(mineCarts[currentMineCart.x + 1, currentMineCart.y]);
                    }
                    if (currentMineCart.y > 0)
                    {
                        if (mineCarts[currentMineCart.x + 1, currentMineCart.y - 1] != null)
                        {
                            adjacentMineCarts.Add(mineCarts[currentMineCart.x + 1, currentMineCart.y - 1]);
                        }
                    }
                }
            }

            return adjacentMineCarts;
        }

        private String randomLetter()
        {
            String randomLetter = "e";

            double randomNumber = rand.NextDouble();

            if ((bonusSeed.Count > 0) && (randomNumber > 0.5))
            {
                randomNumber = rand.NextDouble();
                randomLetter = bonusSeed[(int)(randomNumber * bonusSeed.Count)].ToString();
                bonusSeed.RemoveAt((int)(randomNumber * bonusSeed.Count));
            }
            else
            {
                randomNumber = rand.NextDouble();

                foreach (KeyValuePair<String, Double> letter in alphabet)
                {
                    if (randomNumber <= letter.Value)
                    {
                        randomLetter = letter.Key;
                        break;
                    }
                }
            }

            return randomLetter;
        }

        public void randomizeCart(MineCart mineCart, Boolean usedTNT)
        {
            mineCart.fontColor = Color.White;
            mineCart.contents = randomLetter().ToUpper();

            if ((alphabet[mineCart.contents.ToLower()] >= 0f / 3f) && (alphabet[mineCart.contents.ToLower()] < 1f / 3f))
            {
                mineCart.mineCartType = MineCartType.Gold1;
            }
            else if ((alphabet[mineCart.contents.ToLower()] >= 1f / 3f) && (alphabet[mineCart.contents.ToLower()] < 2f / 3f))
            {
                mineCart.mineCartType = MineCartType.Gold2;
            }
            else if (alphabet[mineCart.contents.ToLower()] >= 2f / 3f)
            {
                mineCart.mineCartType = MineCartType.Gold3;
            }

            if (!usedTNT)
            {
                if ((numberOfTNT < maxNumberOfTNT) && (rand.NextDouble() < tntChance))
                {
                    if (mineCart.y >= 3)
                    {
                        numberOfTNT++;
                        mineCart.mineCartType = MineCartType.TNT;
                        mineCartsTNT.Add(mineCart);
                    }
                }
            }
        }

        public void randomizeCarts()
        {
            foreach (MineCart mineCart in mineCarts)
            {
                if (mineCart != null)
                {
                    if (mineCart.mineCartType != MineCartType.TNT)
                    {
                        randomizeCart(mineCart, true);
                    }
                    else
                    {
                        if (mineCart.y > 0)
                        {
                            if (mineCarts[mineCart.x, mineCart.y - 1].mineCartType != MineCartType.TNT)
                            {
                                lastMineCarts.Add(mineCarts[mineCart.x, mineCart.y - 1]);
                            }
                        }

                    }
                }

                lastMineCarts.Sort();
                foreach (MineCart minecart in lastMineCarts)
                {
                    removeMineCart(minecart, false);
                }
            }
        }

        public void removeMineCart(MineCart temp, Boolean usedTNT)
        {

            mineCarts[temp.x, temp.y] = null;
            for (int k = temp.y + 1; k < CART_MAX_ROWS; k++)
            {
                if (mineCarts[temp.x, k] != null)
                {
                    mineCarts[temp.x, k - 1] = mineCarts[temp.x, k];
                    mineCarts[temp.x, k - 1].fontColor = Color.White;
                    mineCarts[temp.x, k - 1].x = temp.x;
                    mineCarts[temp.x, k - 1].y = k - 1;
                    mineCarts[temp.x, k] = null;
                }
            }

            temp.position.X = CART_OFFSET_X + (temp.x * CART_SPACING_X);
            temp.position.Y = -CART_OFFSET_Y - ((CART_MAX_ROWS - temp.y) * CART_SPACING_Y);


            if ((temp.x % 2) == 0)
            {
                temp.y = CART_MAX_ROWS - 2;
            }
            else
            {
                temp.y = CART_MAX_ROWS - 1;
            }

            randomizeCart(temp, usedTNT);

            mineCarts[temp.x, temp.y] = temp;
        }
    }
}
