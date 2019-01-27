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
        musicNode0: {
            default: null,
            type: cc.Node
        },
        musicNode1: {
            default: null,
            type: cc.Node
        },
    },

    // LIFE-CYCLE CALLBACKS:

    onLoad () {
        this.musicUrl=new Array();
        for(let i=0; i < 3; i++) {     
            this.musicUrl[i]= cc.url.raw('resources/audio/m'+i+'.mp3');
            cc.log("musicUrl "+this.musicUrl[i]);
            cc.audioEngine.preload(this.musicUrl[i]);
        }
        this.volume =1;
        this.volumeWant =1;
        this.musicId=-1;
        this.musicIdWant=-1;
        this.musicPlaying =1;
        this.setMusic(0);    
    },
    start () {

    },
    playSound(sid)
    {
        if(this.musicPlaying==1)
        {
            cc.audioEngine.playEffect(cc.url.raw('resources/audio/s'+sid+'.mp3'), false);
        }
    },
    setMusic(mid)
    {
        if(this.musicPlaying==1)
        {
            if(this.musicId>=0)//有需要淡出的
            {
                this.volumeWant=0; 
                this.musicIdWant = mid;
            }
            else
            {
                this.musicId= mid;
                this.musicNow = cc.audioEngine.play(this.musicUrl[this.musicId], true, this.volume );
            }   
        }
    },
    stopMusic()
    {
        if(this.musicPlaying==1)
        {
            cc.audioEngine.stop(this.musicNow);
            cc.log("stopMusic ");
        }
    },
    musicOn()
    {
        this.musicNode1.x=2000;
        this.musicNode0.x=0;
        this.volume = 0.05;
        this.musicNow = cc.audioEngine.play(this.musicUrl[this.musicId], true, this.volume );
        this.volumeWant=1;
        this.musicPlaying=1;
    },
    musicOff()
    {
        this.musicNode0.x=2000;
        this.musicNode1.x=0;
        this.volumeWant=0; 
    //    this.setMusic(2);
    },
     update (dt) {
        if(this.musicPlaying==1)
        {
            if(this.volume<this.volumeWant)
            {
                this.volume+=0.1;
                if(this.volume>1)
                {
                    this.volume=1;
                }
                cc.audioEngine.setVolume(this.musicNow, this.volume);
            }
            else if(this.volume>this.volumeWant)
            {
                this.volume-=0.025;
                cc.audioEngine.setVolume(this.musicNow, this.volume);
                if(this.volume<0)
                {
                    this.volume=0;
                    cc.audioEngine.stop(this.musicNow);
                    if(this.musicNode1.x==0)
                    {
                        this.musicPlaying=0;
                    }                    
                    if(this.musicIdWant>=0)
                    {
                        this.musicId=this.musicIdWant;
                        this.musicIdWant=-1;
                        this.volumeWant=1;
                        this.musicNow = cc.audioEngine.play(this.musicUrl[this.musicId], true, this.volume );
                    }
                }
            }
        }
     },
});
