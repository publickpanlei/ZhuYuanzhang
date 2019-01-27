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
        yearLabel: {
            default: null,
            type: cc.Label
        },
        storyLabel: {
            default: null,
            type: cc.Label
        },
        textLabel: {
            default: null,
            type: cc.Label
        },
        headSprite: {
            default: null,
            type: cc.Sprite
        },
        yearNode: {
            default: null,
            type: cc.Node
        },
        headNode: {
            default: null,
            type: cc.Node
        },
        textNode: {
            default: null,
            type: cc.Node
        },
        roadNode: {
            default: null,
            type: cc.Node
        },
    },

    // LIFE-CYCLE CALLBACKS:

    // onLoad () {},

    start () {

    },
    init(story,text,headId)
    {
        cc.log("story headId "+headId);
        // if(year!="")
        // {
        //     this.yearLabel.string=year+"年";
        // }          
        this.storyLabel.string=story;
        this.textLabel.string=text;
        var realUrl =cc.url.raw('resources/head/'+headId+'.jpg');
        cc.log("story realUrl "+realUrl);
    //    this.texture =cc.textureCache.addImage(realUrl); 
    //    this.headSprite.spriteFrame.setTexture(this.texture);
        this.headSprite.spriteFrame=new cc.SpriteFrame(realUrl);
    },
    toRight()
    {
        cc.log("story toRight ");
        this.yearNode.x=-this.yearNode.x;
        this.headNode.x=-this.headNode.x;
        this.textNode.x=-this.textNode.x;
        this.roadNode.x=-this.roadNode.x;
    },
    // update (dt) {},
});
