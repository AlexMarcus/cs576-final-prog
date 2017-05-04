package com.example.myfirstapp;

import android.content.Context;
import android.content.res.TypedArray;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.support.v4.view.MotionEventCompat;
import android.util.AttributeSet;
import android.util.Log;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.MotionEvent;
import android.view.View;
import android.widget.Button;
import android.widget.PopupWindow;

import java.util.ArrayList;

import static com.example.myfirstapp.R.layout.activity_radar;

/**
 * Created by alexmarcus on 4/18/17.
 */



public class Radar extends View {

    int count = 0;
    private radarListener local;
    private Paint radarPaint;
    private ArrayList<point> points;
    private ArrayList<ls> lines;

    private float MAX_DIST;

    public class ls{
        public float init_x;
        public float init_y;
        public float final_x;
        public float final_y;
    }

    public class point{
        public float x;
        public float y;
    }

    public Radar(Context context, AttributeSet attrs) {
        super(context, attrs);
        MAX_DIST = 200;
        points = new ArrayList<point>();
        lines = new ArrayList<ls>();
        radarPaint = new Paint(Paint.ANTI_ALIAS_FLAG);
        radarPaint.setStyle(Paint.Style.FILL);

    }

    public void setradarListener(radarListener rl){
        this.local = rl;
    }

    public radarListener getradarListener(){
        return this.local;
    }


    @Override
    public boolean onTouchEvent(MotionEvent event){

        int action = MotionEventCompat.getActionMasked(event);

        float init_x =0;
        float init_y =0;

        switch(action) {
            case (MotionEvent.ACTION_DOWN) :

                if(count < 5) {
                    init_x = event.getX();
                    init_y = event.getY();
                    float centerx = this.getX() + this.getWidth()  / 2;
                    float centery = this.getY() + this.getHeight() / 2;
                    float dist2 = (((centerx - init_x)*(centerx - init_x))+((centery - init_y)*(centery - init_y)));
                    float dist = (float) Math.sqrt(dist2);
                    if(dist > MAX_DIST) {
                        point p = new point();
                        ls l = new ls();

                        p.x = init_x;
                        p.y = init_y;
                        l.init_x = init_x;
                        l.init_y = init_y;
                        l.final_y = 0;
                        l.final_x = 0;
                        points.add(p);
                        lines.add(l);
                        //current = l;
                        if(getradarListener() != null){
                            getradarListener().onRadarTouch(0);
                        }
                        postInvalidate();
                    }
                    else {
                        if(getradarListener() != null){
                            getradarListener().onRadarTouch(-2);
                        }
                        return false;
                    }
                    return true;
                }
                else{
                    if(getradarListener() != null){
                        getradarListener().onRadarTouch(-1);
                    }
                    return false;
                }

            case (MotionEvent.ACTION_MOVE) :

                ls l = lines.get(count);
                float mid_x = event.getX();
                float mid_y = event.getY();

                float m = (mid_y - l.init_y)/(mid_x - l.init_x);
                float b = l.init_y - l.init_x*m;

                float dist2 = (((mid_y - l.init_y)*(mid_y - l.init_y))+((mid_x - l.init_x)*(mid_x - l.init_x)));
                float dist = (float) Math.sqrt(dist2);

                float des_dist = dist*100 + dist*dist*2;

                if(des_dist > 300000) des_dist = 300000;

                float x;
                if((l.init_x - mid_x) <= 0) {
                    x = l.init_x + (float) Math.sqrt((des_dist) / (1 + (m * m)));
                } else {
                    x = l.init_x - (float) Math.sqrt((des_dist) / (1 + (m * m)));
                }
                System.out.println("X:");
                System.out.println(x);
                float y = x*m + b;
                System.out.println("Y:");
                System.out.println(y);

                l.final_x = x;
                l.final_y = y;
                postInvalidate();

                return true;

            case (MotionEvent.ACTION_UP) :
                count++;
                postInvalidate();

                return true;

            case (MotionEvent.ACTION_CANCEL) :

                return true;
            case (MotionEvent.ACTION_OUTSIDE) :

                return true;
            default :
                return super.onTouchEvent(event);
        }
    }

    @Override
    public void onDraw(Canvas canvas) {

        super.onDraw(canvas);

        radarPaint.setARGB(255,0,0,0);
        float centerx = this.getX() + this.getWidth()  / 2;
        float centery = this.getY() + this.getHeight() / 2;
        canvas.drawCircle(centerx,centery,25,radarPaint);
        radarPaint.setStyle(Paint.Style.STROKE);
        canvas.drawCircle(centerx,centery,MAX_DIST,radarPaint);
        radarPaint.setARGB(255,255,255,255);
        radarPaint.setStyle(Paint.Style.FILL);

        for(point p : points){

            canvas.drawCircle(p.x,p.y,20,radarPaint);
        }
        for(ls l : lines){
            //System.out.println(l.final_y);
            //System.out.println(l.final_x);
            radarPaint.setStrokeWidth(10);
            if(l.final_y != 0 && l.final_x != 0) {
                canvas.drawLine(l.init_x, l.init_y, l.final_x, l.final_y, radarPaint);
            }
        }

    }

    public interface radarListener{
        void onRadarTouch(float val);
    }


}

