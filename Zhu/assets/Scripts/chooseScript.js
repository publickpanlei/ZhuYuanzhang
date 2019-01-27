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
        leftLabel: {
            default: null,
            type: cc.Label
        },
        rightLabel: {
            default: null,
            type: cc.Label
        },
        photoSprite: {
            default: null,
            type: cc.Sprite
        },
        roadNode0: {
            default: null,
            type: cc.Node
        },
        roadNode1: {
            default: null,
            type: cc.Node
        },
    },

    // LIFE-CYCLE CALLBACKS:

    // onLoad () {},

    start () {

    },
    init(left,right,photoId)
    {
        cc.log("choose photoId "+photoId);
        this.leftLabel.string=left;
        this.rightLabel.string=right;
        var realUrl =cc.url.raw('resources/photos/'+photoId+'.jpg');
        cc.log("choose realUrl "+realUrl);
    //    this.texture =cc.textureCache.addImage(realUrl); 
    //    this.photoSprite.spriteFrame.setTexture(this.texture);
        this.photoSprite.spriteFrame=new cc.SpriteFrame(realUrl);
    },
    toRight()
    {
        this.roadNode0.x=-this.roadNode0.x;
        this.roadNode1.x=-this.roadNode1.x;
    },
    // update (dt) {},
});
