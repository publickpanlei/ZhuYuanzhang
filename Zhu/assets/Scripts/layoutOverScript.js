// Learn cc.Class:
//  - [Chinese] http://docs.cocos.com/creator/manual/zh/scripting/class.html
//  - [English] http://www.cocos2d-x.org/docs/creator/en/scripting/class.html
// Learn Attribute:
//  - [Chinese] http://docs.cocos.com/creator/manual/zh/scripting/reference/attributes.html
//  - [English] http://www.cocos2d-x.org/docs/creator/en/scripting/reference/attributes.html
// Learn life-cycle callbacks:
//  - [Chinese] http://docs.cocos.com/creator/manual/zh/scripting/life-cycle-callbacks.html
//  - [English] http://www.cocos2d-x.org/docs/creator/en/scripting/life-cycle-callbacks.html

cc.Class({
    extends: cc.Component,

    properties: {
        // foo: {
        //     // ATTRIBUTES:
        //     default: null,        // The default value will be used only when the component attaching
        //                           // to a node for the first time
        //     type: cc.SpriteFrame, // optional, default is typeof default
        //     serializable: true,   // optional, default is true
        // },
        // bar: {
        //     get () {
        //         return this._bar;
        //     },
        //     set (value) {
        //         this._bar = value;
        //     }
        // },
        storyPrefab: {
            default: null,
            type: cc.Prefab
        },
        choosePrefab: {
            default: null,
            type: cc.Prefab
        },
        photoSprite: {
            default: null,
            type: cc.Sprite
        },
        contentNode: {
            default: null,
            type: cc.Node
        },
        buttonNode: {
            default: null,
            type: cc.Node
        },
    },

    // LIFE-CYCLE CALLBACKS:

     onLoad () {
        this.storyArr=new Array();
        this.storyArrRot=new Array();
        this.storyId=0;
        this.nowY=-600;
        cc.log("onLoad layoutOverScript");
     },

    start () {

    },
    
    init (game,storyArr,storyArrRot,storyId) {
        cc.log("init layoutOverScript");
        this.game=game;
        this.storyArr=storyArr;
        this.storyArrRot=storyArrRot;
        this.storyId=storyId;
        this.photoId=this.game.thisCsv_photoid[this.storyArr[this.storyId-1]]; 
    //    cc.log("over this.photoId "+this.photoId);
        var realUrl =cc.url.raw('resources/photos/'+this.photoId+'.jpg');
        this.texture =cc.textureCache.addImage(realUrl); 
        this.photoSprite.spriteFrame.setTexture(this.texture);

        for(let i=0; i < this.storyId; i++) {        
        //    cc.log("over this.game.thisCsv_left[i]"+this.game.thisCsv_left[this.storyArr[x]]);
            if(this.game.thisCsv_left[this.storyArr[i]]=="")//story
            {
                var newStory = cc.instantiate(this.storyPrefab);
                this.contentNode.addChild(newStory);
                newStory.setPositionY(this.nowY);
                newStory.getComponent("storyScript").init(
                //    this.game.thisCsv_year[this.storyArr[i]],
                    this.game.thisCsv_status[this.storyArr[i]],
                    this.game.thisCsv_text[this.storyArr[i]],
                    this.game.thisCsv_summary[this.storyArr[i]]);
                cc.log("over this.storyArrRot "+this.storyArrRot[i]);
                if(this.storyArrRot[i]==1)
                {
                    newStory.getComponent("storyScript").toRight();
                }
            //    
                this.nowY-=170;
            }
            else//choose
            {
                var newChoose = cc.instantiate(this.choosePrefab);
                this.contentNode.addChild(newChoose);
                newChoose.setPositionY(this.nowY);
                newChoose.getComponent("chooseScript").init(
                    this.game.thisCsv_left[this.storyArr[i]],
                    this.game.thisCsv_right[this.storyArr[i]],
                    this.game.thisCsv_photoid[this.storyArr[i]]);
                cc.log("over this.storyArrRot "+this.storyArrRot[i]);
                if(this.storyArrRot[i]==1)
                {
                    newChoose.getComponent("chooseScript").toRight();
                }
                this.nowY-=340;
            }
            cc.log("over this.storyArr "+this.storyArr[i]);
        } 
        this.contentNode.height=-this.nowY+200;
        this.buttonNode.y=this.nowY-100;
    },
    // update (dt) {},
});
