package com.example.secondlife.ui.productDetails;

import androidx.activity.OnBackPressedCallback;
import androidx.lifecycle.ViewModelProvider;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.navigation.fragment.NavHostFragment;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RatingBar;
import android.widget.TextView;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.ActivityProductDetailsBinding;
import com.example.secondlife.databinding.ProductDetailsFragmentBinding;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

public class ProductDetailsFragment extends Fragment {

    private ProductDetailsFragmentBinding binding;
    private ProductDetailsViewModel mViewModel;
    LocalData localData = LocalData.GetInstance();
    View view;
    Product product = null;
    Photo photo = null;

    public static ProductDetailsFragment newInstance() {
        return new ProductDetailsFragment();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {

        Gson gson = new Gson();

        Bundle bundle = getArguments();
        product = (Product) bundle.getSerializable("product");
        // photo = (Photo) bundle.getSerializable("photo");

        binding = ProductDetailsFragmentBinding.inflate(inflater, container, false);
        view = binding.getRoot();

        // Rating
        callRateButton();


        // Pour les info du Product
        Picasso.get().load(R.drawable.ic_baseline_image_search_24).into(binding.productImg);
        binding.productName.setText(product.getName());
        binding.productDesc.setText(product.getDescription());
        if (localData.getUserId() > -1) {
            binding.btnBuy.setVisibility(View.VISIBLE);
            binding.ratingBar.setVisibility(View.VISIBLE);
            binding.rateButton.setVisibility(View.VISIBLE);
            binding.textRatingBar.setVisibility(View.VISIBLE);
            binding.editTextComment.setVisibility(View.VISIBLE);
            binding.textComment.setVisibility(View.VISIBLE);
        }
        Log.d("product", product.getName());


        return view;
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        mViewModel = new ViewModelProvider(this).get(ProductDetailsViewModel.class);
        // TODO: Use the ViewModel
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

    public void callRateButton(){
        RatingBar ratingbar = binding.ratingBar;
        Button rateButton = binding.rateButton;

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