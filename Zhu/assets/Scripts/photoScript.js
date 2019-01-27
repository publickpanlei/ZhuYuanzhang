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
        photoMoveNode: {
            default: null,
            type: cc.Node
        },
        photoBlack: {
            default: null,
            type: cc.Node
        },
        leftNode: {
            default: null,
            type: cc.Node
        },
        rightNode: {
            default: null,
            type: cc.Node
        },
        moveLabel: {
            default: null,
            type: cc.Label
        },
    },

    // LIFE-CYCLE CALLBACKS:
    init(game) {
		this.game=game;
    },
    setChoose(ch)
    {
        this.choose=ch;
    //    console.log("choose",this.choose);
        if(this.choose==0)
        {
            this.photoBlack.opacity = 0;
        }
        else if(this.choose==1)
        {
            this.photoBlack.opacity = 192;
        }
    },
    onLoad () {
        this.choose = 0;
        this.touchStartX = 0;
        this.touchStartY = 0;
        this.movingX = 0;
        this.node.on("touchstart", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
        //    this.node.x = touchPoint.x;
        //    this.touchStartX = touchPoint.x;
        //    this.touchStartY = touchPoint.y;
		//	console.log("touchstart",touchPoint);
        });
        this.node.on("touchmove", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
        //    this.node.x = touchPoint.x;            
        //    this.movingX = touchPoint.x- this.touchStartX;
        //    this.node.x = this.movingX;
            var x = event.getDeltaX();
            var y = event.getDeltaY();
            this.node.x += x;
        //    this.node.y += y;
            this.photoMove();
		//	console.log("touchmove",touchPoint);
        });
        this.node.on("touchend", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
        //    this.node.x = touchPoint.x;
            this.photoFire();
		//	console.log("touchend",touchPoint);
        });
        this.node.on("touchcancel", (event) => {
            //将世界坐标转化为本地坐标
        //    let touchPoint = this.node.convertToNodeSpace(event.getLocation());
        //    this.node.x = touchPoint.x;
            this.photoFire();                     
		//	console.log("touchcancel",touchPoint);
        });
    },
    photoMove: function(){
        //    cc.log("this.photoContent "+this.photoContent.x);
            this.moveLabel.string=Math.round(this.node.x);
            this.photoRot=this.node.x*0.05;
            this.node.rotation=this.photoRot;
            this.photoBlack.rotation=-this.photoRot;
        //    var radians = this.photoRot * (Math.PI/180);
        //    console.log("photoRot",this.photoRot);
        //    console.log("radians",radians);    
            this.ys=Math.abs(Math.sin(this.photoRot * (Math.PI/180)));
            if(this.ys<0.05)
            {
                this.photoBlack.y = 112+400;
            }
            else if(this.ys<0.1)
            {
                this.photoBlack.y = 112+150*this.ys+(0.1-this.ys)*4000;
            }
            else{
                this.photoBlack.y = 112+150*this.ys;//112+15
            }
            this.leftOpacity=-this.node.x*10-300;
            this.rightOpacity=this.node.x*10-300;
            if(this.leftOpacity<0)
            {
                this.leftOpacity=0;
            }
            else if(this.leftOpacity>255)
            {
                this.leftOpacity=255;            
            }
            if(this.rightOpacity<0)
            {
                this.rightOpacity=0;
            }
            else if(this.rightOpacity>255)
            {
                this.rightOpacity=255;
            }
            this.leftNode.opacity=this.leftOpacity;
            this.rightNode.opacity=this.rightOpacity;

        },
    photoFire: function(){
        cc.log("photoFire "+this.node.x);
        if(this.choose==0)
        {
            this.moveLength=110;
        }
        else{
            this.moveLength=230;
        }
        if(this.node.x<-this.moveLength)
        {
            this.game.goLeft();
            this.photoThrowLeft();
        }
        else if(this.node.x>this.moveLength)
        {
            this.game.goRight();
            this.photoThrowRight();
        }
        else
        {
        //    this.node.x = 0;
        //    this.node.rotation=0;   
            this.photoBackZero();
        }
        this.photoBlack.rotation=0;
        this.photoBlack.y=512;
        this.photoBlack.x=0;
        this.leftOpacity=0;
        this.rightOpacity=0;
        this.leftNode.opacity=this.leftOpacity;
        this.rightNode.opacity=this.rightOpacity;
        this.moveLabel.string=0;
    },
    photoBackZero: function(){
        // 创建一个移动动作
        var action = cc.moveTo(0.1, 0, 0);
        // 执行动作
        this.node.runAction(action);

        var action2 = cc.rotateTo(0.1, 0);
        // 执行动作
        this.node.runAction(action2);
    },
    photoThrowLeft: function(){
        // 创建一个移动动作
        var action = cc.moveBy(0.23, -350, -70);
    //    this.photoMoveNode.anchorY=0.5;
     //   var actionR = cc.rotateBy(0.23, -90);
        // 执行动作
        this.photoMoveNode.runAction(action);
    //    this.photoMoveNode.runAction(actionR);
    },
    photoThrowRight: function(){
        // 创建一个移动动作
        var action = cc.moveBy(0.23, 350, -70);
    //    this.photoMoveNode.anchorY=0.5;
    //    var actionR = cc.rotateBy(0.23, 90);
        // 执行动作
        this.photoMoveNode.runAction(action);
    //    this.photoMoveNode.runAction(actionR);
    },
    goOk: function(){
        cc.log("photo goOk ");
        this.game.goOk();
        this.photoMoveNode.x=0;
        this.photoMoveNode.y=-402;
        this.node.x = 0;
        this.node.rotation=0;  
    },
    start () {

    },

    // update (dt) {},
});
