package com.example.secondlife.ui;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.ActivityProductDetailsBinding;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

public class ProductDetails extends AppCompatActivity {

    private ActivityProductDetailsBinding binding;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_product_details);

        Gson gson = new Gson();
        Product product = gson.fromJson(getIntent().getExtras().getString("product"), Product.class);
        Photo photo = gson.fromJson(getIntent().getExtras().getString("photo"), Photo.class);

        binding = ActivityProductDetailsBinding.inflate(getLayoutInflater());
        View view = binding.getRoot();

        // Pour les info du Product

        //Picasso.get().load(photo.getUrl()).into((ImageView)findViewById(R.id.product_img));
        Picasso.get().load(R.drawable.ic_baseline_image_search_24).into((ImageView)findViewById(R.id.product_img));
        ((TextView)findViewById(R.id.product_name)).setText(product.getName());
        ((TextView)findViewById(R.id.product_desc)).setText(product.getDescription());
        if (((LocalData)getApplication()).getUserId() > -1) {
            findViewById(R.id.btn_buy).setVisibility(View.VISIBLE);
        }
        Log.d("product", product.getName());
    }
}