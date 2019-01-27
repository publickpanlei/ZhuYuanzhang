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
        thisLabel: {
            default: null,
            type: cc.Label
        },
        thisNode: {
            default: null,
            type: cc.Node
        },
    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.thisNode.on("touchmove", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
            var x = event.getDeltaX();
            this.thisNode.x += x;
            this.photoMove();
		//	console.log("touchmove",touchPoint);
        });
        this.thisNode.on("touchend", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
        //    this.node.x = touchPoint.x;
            this.photoFire();
		//	console.log("touchend",touchPoint);
        });
        this.thisNode.on("touchcancel", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
        //    this.node.x = touchPoint.x;
            this.photoFire();                     
		//	console.log("touchcancel",touchPoint);
        });
    },
    photoMove: function(){
        //    cc.log("this.photoContent "+this.photoContent.x);
            this.photoRot=this.node.x*0.05;
            this.thisNode.rotation=this.photoRot;

        },
    photoFire: function(){
        cc.log("photoFire "+this.thisNode.x);
        if(this.thisNode.x<-120)
        {
        //    this.game.goLeft();
            this.photoThrowLeft();
            this.thisNode.getComponent(cc.Animation).play("textAni");
            this.node.getComponent(cc.Animation).play("photoAni");
        }
        else if(this.thisNode.x>120)
        {
        //    this.game.goRight();
            this.photoThrowRight();
            this.thisNode.getComponent(cc.Animation).play("textAni");
            this.node.getComponent(cc.Animation).play("photoAni");
        }
        else
        {
        //    this.node.x = 0;
        //    this.node.rotation=0;   
            this.photoBackZero();
        }
    },
    photoBackZero: function(){
        // 创建一个移动动作
        var action = cc.moveTo(0.1, 0, 0);
        // 执行动作
        this.thisNode.runAction(action);

        var action2 = cc.rotateTo(0.1, 0);
        // 执行动作
        this.thisNode.runAction(action2);
    },
    photoThrowLeft: function(){
        // 创建一个移动动作
        var action = cc.moveBy(0.23, -350, -70);
    //    this.photoMoveNode.anchorY=0.5;
     //   var actionR = cc.rotateBy(0.23, -90);
        // 执行动作
        this.thisNode.runAction(action);
    //    this.photoMoveNode.runAction(actionR);
    },
    photoThrowRight: function(){
        // 创建一个移动动作
        var action = cc.moveBy(0.23, 350, -70);
    //    this.photoMoveNode.anchorY=0.5;
    //    var actionR = cc.rotateBy(0.23, 90);
        // 执行动作
        this.thisNode.runAction(action);
    //    this.photoMoveNode.runAction(actionR);
    },
    goOk: function(){
        cc.log("photo goOk ");
    //    this.game.goOk();
        this.thisNode.x=0;
        this.thisNode.y=0;
        this.thisNode.rotation=0;  
    },
    start () {

    },

    // update (dt) {},
});
