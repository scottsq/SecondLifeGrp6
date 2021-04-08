package com.example.secondlife.ui;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.RatingBar;
import android.widget.TextView;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.ActivityProductDetailsBinding;
import com.example.secondlife.databinding.FragmentProfilBinding;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.Rating;
import com.example.secondlife.model.User;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class ProductDetails extends AppCompatActivity {

    private ActivityProductDetailsBinding binding;
    LocalData localData = LocalData.GetInstance();
    View view;
    Product product = null;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_product_details);

        Gson gson = new Gson();
        product = gson.fromJson(getIntent().getExtras().getString("product"), Product.class);
        Photo photo = gson.fromJson(getIntent().getExtras().getString("photo"), Photo.class);

        binding = ActivityProductDetailsBinding.inflate(getLayoutInflater());
        view = binding.getRoot();

        // Rating
        callRateButton();


        // Pour les info du Product

        //Picasso.get().load(photo.getUrl()).into((ImageView)findViewById(R.id.product_img));
        Picasso.get().load(R.drawable.ic_baseline_image_search_24).into((ImageView)findViewById(R.id.product_img));
        ((TextView)findViewById(R.id.product_name)).setText(product.getName());
        ((TextView)findViewById(R.id.product_desc)).setText(product.getDescription());
        if (localData.getUserId() > -1) {
            findViewById(R.id.btn_buy).setVisibility(View.VISIBLE);
        }
        Log.d("product", product.getName());
    }


//    public View.OnClickListener callRateButton() {
//        return new View.OnClickListener() {
//            @Override
//            public void onClick(View view){
//                Log.v("test rate" , "test");
//                Log.v("test rate" , String.valueOf(binding.ratingBar.getNumStars()));
//            }
//        };
//    }

    public void callRateButton(){
        RatingBar ratingbar =findViewById(R.id.ratingBar);
        Button rateButton = findViewById(R.id.rateButton);

        rateButton.setOnClickListener(new View.OnClickListener(){

            @Override
            public void onClick(View arg0) {
                String rating = String.valueOf(ratingbar.getRating());
                //user id
                Log.v("test user rate" , String.valueOf(localData.getUserId()));
                //product id
                Log.v("test product rate" , String.valueOf(product.getId()));


                Log.v("test rate" , String.valueOf(ratingbar.getRating()));
                rateButton.setEnabled(false);
            }

        });
    }
}