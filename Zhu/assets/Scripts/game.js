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
        photoScript: require('photoScript'),
        musicScript: require('musicScript'),
        layoutOverScript: require('layoutOverScript'),
		photoNode: {
            default: null,
            type: cc.Node
        },
		photoSprite: {
            default: null,
            type: cc.Sprite
        },
		photoContent: {
            default: null,
            type: cc.Node
        },	
        photoBackNode: {
            default: null,
            type: cc.Node
        },	
		talkLabel: {
            default: null,
            type: cc.Label
        },
		nameLabel: {
            default: null,
            type: cc.Label
        },
		textLabel: {
            default: null,
            type: cc.Label
        },
		leftLabel: {
            default: null,
            type: cc.Label
        },
		rightLabel: {
            default: null,
            type: cc.Label
        },
        statusLabel: {
            default: null,
            type: cc.Label
        },
        layoutOver: {
            default: null,
            type: cc.Node
        },

    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.thisCellId=0;
        this.thisCellLeftId=0;
        this.thisCellRightId=0;
		this.thisCell=new Array();
        this.cell="";
        this.cellLength=0;
        this.thisLine=new Array();
        this.thisCsv_cellName=new Array();
        this.thisCsv_talk=new Array();
        this.thisCsv_text=new Array();
        this.thisCsv_photoid=new Array();
        this.thisCsv_right=new Array();
        this.thisCsv_left=new Array();
        this.thisCsv_emoji=new Array();
        this.thisCsv_music=new Array();
        this.thisCsv_sound=new Array();
        this.thisCsv_name=new Array();
        this.thisCsv_status=new Array();
        this.thisCsv_summary=new Array();
        this.storyArr=new Array();
        this.storyArrRot=new Array();
        this.storyId=0;
        this.storyIf=0;
        this.line="";
        this.lineLength=0;
        this.lineId=0;
        this.lineLeftId=0;
        this.lineRightId=0;
        this.id=0;
        this.leftId=0;
        this.rightId=0;
        this.photoId=0;
        this.photoRightId=0;
        this.thisCellName="";
        this.photoRot=0;
        this.leftOpacity=0;
        this.rightOpacity=0;
        this.photoScript.init(this);
        

        cc.loader.load(cc.url.raw('resources/save/save_0.csv'), (err,object) => {
            // cc.loader.load(cc.url.raw('resources/B3U3XML.json'), function (err,object) {
            // (err,object)为回调方法，得出结果，在此注意，应避免用function，因为这样声明函数作用域会过小，导致其他空间调用此赋值无效
            if (err) {
                cc.log(err);
            }
            else{
                this.loadCsv = object;   // 使用(err,object) =>时的方法	
            //    console.log("loadCsv", this.loadCsv);
                this.csvs = this.loadCsv.split("\n");
            this.cellLength=this.csvs.length-1;	
                for(let x=2; x < this.cellLength; x++) {                  
                    var arr=this.csvs[x].split(",");                
                    this.thisCsv_cellName[x-2]=arr[0];
                    this.thisCsv_talk[x-2]=arr[1];
                    this.thisCsv_text[x-2]=arr[2];
                    this.thisCsv_photoid[x-2]=arr[3];
                    this.thisCsv_right[x-2]=arr[4];
                    this.thisCsv_left[x-2]=arr[5];
                    this.thisCsv_emoji[x-2]=arr[8];
                    this.thisCsv_music[x-2]=arr[9];
                    this.thisCsv_sound[x-2]=arr[10];
                    this.thisCsv_name[x-2]=arr[11];
                    this.thisCsv_status[x-2]=arr[12];
                    this.thisCsv_summary[x-2]=arr[13];
                }
            }
            this.init();
        });

        // var self = this; 
    /*    cc.loader.load(cc.url.raw('resources/save/save_0.json'), (err,object) => {
            // cc.loader.load(cc.url.raw('resources/B3U3XML.json'), function (err,object) {
            // (err,object)为回调方法，得出结果，在此注意，应避免用function，因为这样声明函数作用域会过小，导致其他空间调用此赋值无效
            if (err) {
                cc.log(err);
            }
            else{
                this.cell = object.cells;   // 使用(err,object) =>时的方法	
                this.cellLength=this.cell.length;		
				for (let x=0; x < this.cellLength; x++) {
					this.thisCell[x]=this.cell[x];  
				}
                this.init();
            }
        });*/
        cc.loader.load(cc.url.raw('resources/save/line_0.json'), (err,object) => {
            // cc.loader.load(cc.url.raw('resources/B3U3XML.json'), function (err,object) {
            // (err,object)为回调方法，得出结果，在此注意，应避免用function，因为这样声明函数作用域会过小，导致其他空间调用此赋值无效
            if (err) {
                cc.log(err);
            }
            else{
                this.line = object.cells;   // 使用(err,object) =>时的方法		
                this.lineLength=this.line.length;		
				for (let x=0; x < this.lineLength; x++) {
					this.thisLine[x]=this.line[x];
                }
            }
            this.initLeftRight();
            this.initNextPhoto(); 
        });
	},
    init: function(){
            this.nameLabel.string = this.thisCsv_name[0];
            this.statusLabel.string = this.thisCsv_status[0];
            this.talkLabel.string = this.thisCsv_talk[0];
            this.textLabel.string = this.thisCsv_text[0];	
            this.leftLabel.string = this.thisCsv_left[0];
            this.rightLabel.string = this.thisCsv_left[0];
            this.photoId=this.thisCsv_photoid[0]; 
            cc.log("photoId "+this.photoId);
        //    var realUrl =cc.url.raw('resources/photos/photo_'+this.photoId+'.jpg');
            var realUrl =cc.url.raw('resources/photos/'+this.photoId+'.jpg');
            this.texture =cc.textureCache.addImage(realUrl); 
            this.photoSprite.spriteFrame.setTexture(this.texture);

        },
    start () {

    },
    goLeft:function(){        
        this.id = this.leftId;
        this.lineId = this.lineLeftId;
        this.thisCellId = this.thisCellLeftId;
        this.goNew();
	},
	goRight:function(){
        this.id = this.rightId;
        this.lineId = this.lineRightId;
        this.thisCellId = this.thisCellIdRight;
        this.goNew();
    },
    goNew:function(){
        if(this.thisCsv_name[this.thisCellId]!="")
        {
            this.nameLabel.string =  this.thisCsv_name[this.thisCellId];
        } 
        if(this.thisCsv_status[this.thisCellId]!="")
        {
            this.statusLabel.string = this.thisCsv_status[this.thisCellId];  
        }              
        this.storyIf=0;
        if(this.thisCsv_summary[this.thisCellId]!="")
        {
        //    cc.log("记录这一条 "+this.thisCsv_summary[this.thisCellId]+" 这里");
            this.storyIf=1;//是要记录的
            this.storyWant=1;
            this.storyArr[this.storyId] = this.thisCellId;
            if(this.storyId==0)
            {
                this.storyArrRot[this.storyId] = 0;
            }
            this.storyId++;
        }
        this.talkLabel.string = this.thisCsv_talk[this.thisCellId];
        this.textLabel.string = this.thisCsv_text[this.thisCellId];
        this.leftLabel.string = this.thisCsv_left[this.thisCellId];
    //    this.photoId=this.thisCsv_photoid[this.thisCellId]; 
        if(this.thisCsv_right[this.thisCellId]=="")
        {
            this.rightLabel.string = this.thisCsv_left[this.thisCellId];
            this.photoScript.setChoose(0);  
        }
        else
        {
            this.rightLabel.string = this.thisCsv_right[this.thisCellId];
            this.photoScript.setChoose(1);
        }
        
        this.goAni();
    },
    goAni:function(){ 
    //    this.photoContent.x=0;
    //    this.photoNode.setSkewY(10);
    //    this.photoNode.setScaleX(0);
        this.photoNode.getComponent(cc.Animation).play("photoAni02");
        this.photoBackNode.getComponent(cc.Animation).play("fanpai");
        this.talkLabel.getComponent(cc.Animation).play("textAni");
        this.nameLabel.getComponent(cc.Animation).play("textAni");
        this.textLabel.getComponent(cc.Animation).play("textAni");      
        this.statusLabel.getComponent(cc.Animation).play("textAni");
    },
    goOk:function(){ 
        cc.log("game goOk ");
        this.photoContent.x=0;
    //    this.photoNode.setSkewY(10);
        this.photoNode.setScaleX(0);         
        this.photoNode.opacity=0; 
        if(this.rightId != this.leftId)
        {
            if( this.id == this.rightId)//右选
            {
                this.photoSprite.spriteFrame.setTexture(this.textureRight);
            //    cc.log("storyArrRot右选 "+1);
                this.storyArrRot[this.storyId] = 1;
            }
            else//左选
            {
                this.photoSprite.spriteFrame.setTexture(this.texture);
            //    cc.log("storyArrRot左选 "+0);
                this.storyArrRot[this.storyId] = 0;
            }
        }
        else//不选
        {
            this.photoSprite.spriteFrame.setTexture(this.texture);
            if(this.storyIf==1)
            {               
                this.storyArrRot[this.storyId] = this.storyArrRot[this.storyId-1];
            //    cc.log("storyArrRot继续 "+this.storyArrRot[this.storyId]);
            }
        }       
        if(this.thisCsv_cellName[this.thisCellId]=="PC001A")
        {
            this.musicScript.setMusic(1);
        }
        else if(this.thisCsv_cellName[this.thisCellId]=="PC001B")
        {
            this.musicScript.setMusic(2);
        }
        this.initLeftRight();
        this.initNextPhoto();  
        if(this.id==0)
        {
            this.layoutOver.active = true;          
            this.layoutOverScript.init(this,this.storyArr,this.storyArrRot,this.storyId);
            this.storyId=0;
        }
    },
    initLeftRight:function(){  
        cc.log("initLeftRight");
        if(this.thisLine[this.lineId].leftId==-1)
        {
            this.leftId=0;
            this.rightId=0;
            this.lineRightId=0;
            this.lineLeftId=0;
            this.thisCellIdRight=0;
            this.thisCellLeftId=0;
            this.photoId=this.thisCsv_photoid[0];
            this.photoRightId=this.photoId;
        }
        else
        {
            this.leftId=this.thisLine[this.lineId].leftId;
            for (let i=0; i < this.lineLength; i++) {
                if(this.thisLine[i].id==this.leftId)
                {
                    this.thisCellName=this.thisLine[i].cellName;
                    this.lineLeftId=i;                    
                    break;
                }
            }  
            for (let i=0; i < this.cellLength; i++) {
                if(this.thisCsv_cellName[i]==this.thisCellName)
                {
                    this.thisCellLeftId=i;
                    this.photoId=this.thisCsv_photoid[this.thisCellLeftId]; 
                    break;
                }
            } 
            if(this.thisLine[this.lineId].rightId==-1)
            {
                this.rightId=this.leftId;
                this.lineRightId=this.lineLeftId;
                this.thisCellIdRight=this.thisCellLeftId;
                this.photoRightId=this.photoId;
            }
            else
            {
                this.rightId=this.thisLine[this.lineId].rightId;
                for (let i=0; i < this.lineLength; i++) {
                    if(this.thisLine[i].id==this.rightId)
                    {
                        this.thisCellName=this.thisLine[i].cellName;
                        this.lineRightId=i;
                        break;
                    }
                }  
                for (let i=0; i <  this.cellLength; i++) {
                    if(this.thisCsv_cellName[i]==this.thisCellName)
                    {
                        this.thisCellIdRight=i;
                        this.photoRightId=this.thisCsv_photoid[this.thisCellIdRight]; 
                        break;
                    }
                } 
                this.musicScript.playSound(0);
            }
        }
    },
    initNextPhoto:function(){  
        var realUrl =cc.url.raw('resources/photos/'+this.photoId+'.jpg');
        this.texture =cc.textureCache.addImage(realUrl);
        if(this.rightId!=this.leftId)
        {
            var realUrl2 =cc.url.raw('resources/photos/'+this.photoRightId+'.jpg');
            this.textureRight =cc.textureCache.addImage(realUrl2);
        }
    },
    // update (dt) {},
});
