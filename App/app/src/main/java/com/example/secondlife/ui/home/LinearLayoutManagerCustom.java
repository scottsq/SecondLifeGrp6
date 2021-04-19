package com.example.secondlife.ui.home;

import android.content.Context;
import android.util.AttributeSet;

import androidx.recyclerview.widget.LinearLayoutManager;

public class LinearLayoutManagerCustom extends LinearLayoutManager {
    private boolean canScrollV = true;

    public LinearLayoutManagerCustom(Context context) {
        super(context);
    }

    public LinearLayoutManagerCustom(Context context, int orientation, boolean reverseLayout) {
        super(context, orientation, reverseLayout);
    }

    public LinearLayoutManagerCustom(Context context, AttributeSet attrs, int defStyleAttr, int defStyleRes) {
        super(context, attrs, defStyleAttr, defStyleRes);
    }

    @Override
    public boolean canScrollVertically() {
        return canScrollV;
    }

    public void setCanScrollVertically(boolean value) {
        canScrollV = value;
    }
}
