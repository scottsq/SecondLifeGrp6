package com.example.secondlife.ui.productDetails;

import androidx.fragment.app.FragmentTransaction;
import androidx.lifecycle.ViewModelProvider;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.RatingBar;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.ProductDetailsFragmentBinding;
import com.example.secondlife.model.LoginResponse;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.ProductRating;
import com.example.secondlife.model.ProductWithPhoto;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.ProductRatingService;
import com.example.secondlife.network.ProductService;
import com.example.secondlife.ui.profil.ProfilFragment;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.squareup.picasso.Picasso;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class ProductDetailsFragment extends Fragment {

    private ProductDetailsFragmentBinding binding;
    private ProductDetailsViewModel mViewModel;
    private LocalData localData = LocalData.GetInstance();
    private View view;
    private ProductWithPhoto product = null;
    private Photo photo = null;
    private ProductService productService = null;
    private Retrofit retrofit = localData.GetRetrofit();
    private ProductRatingService apiService = retrofit.create(ProductRatingService.class);

    public static ProductDetailsFragment newInstance() {
        return new ProductDetailsFragment();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {

        Bundle bundle = getArguments();
        product = (ProductWithPhoto) bundle.getSerializable("product");
        // photo = (Photo) bundle.getSerializable("photo");

        binding = ProductDetailsFragmentBinding.inflate(inflater, container, false);
        view = binding.getRoot();

        // Rating
        callRateButton();

        // Pour les info du Product
        if (product.getPhotoList() == null || product.getPhotoList().size() == 0){
            Picasso.get().load(R.drawable.ic_baseline_image_search_24).placeholder(R.drawable.ic_baseline_image_search_24).into(binding.productImg);
        }
        else
        {
            Picasso.get().load(product.getPhotoList().get(0).getUrl()).placeholder(R.drawable.ic_baseline_image_search_24).into(binding.productImg);
        }
        binding.productName.setText(product.getProduct().getName());
        binding.productDesc.setText(product.getProduct().getDescription());
        apiService.getAverage(product.getProduct().getId()).enqueue(getRatingStars());
        binding.textOwner.setText("Vendeur: " + product.getProduct().getOwner().getName());
        binding.textPrice.setText("Prix: " + product.getProduct().getPrice() + "€");
        if (localData.getUserId() > -1) {

            apiService.getUserRatingForProduct(product.getProduct().getId(),localData.getUserId()).enqueue(getUserRating());

            binding.btnBuy.setVisibility(View.VISIBLE);
        }
        Log.d("product", product.getProduct().getName());


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
                Log.v("test rating" , String.format("userId: [%d], productId: [%d], rating: [%f]", localData.getUserId(), product.getProduct().getId(), ratingbar.getRating()));

                User user = new User();
                user.setId(localData.getUserId());

                ProductRating productRating = new ProductRating();

                productRating.setUser(user);
                productRating.setProduct(product.getProduct());
                productRating.setStars((int) ratingbar.getRating());
                productRating.setComment(binding.editTextComment.getText().toString());

                apiService.createProductRating(LocalData.GetInstance().getToken(),productRating).enqueue(new Callback<ProductRating>() {
                    @Override
                    public void onResponse(Call<ProductRating> call, Response<ProductRating> response) {
                        ProductRating check = response.body();
                    }

                    @Override
                    public void onFailure(Call<ProductRating> call, Throwable t) {
                        // Afficher message d'erreur
                    }
                });

                rateButton.setEnabled(false);
            }

        });
    }

    private Callback<Float> getRatingStars(){
        return new Callback<Float>() {
            @Override
            public void onResponse(Call<Float> call, Response<Float> response) {
                binding.textStars.setText("Note: " + response.body() +" ⭐");
            }

            @Override
            public void onFailure(Call<Float> call, Throwable t) {

            }
        };
    }

    private Callback<ProductRating> getUserRating(){
        return new Callback<ProductRating>() {
            @Override
            public void onResponse(Call<ProductRating> call, Response<ProductRating> response) {
                if (!response.isSuccessful()){
                    binding.ratingBar.setVisibility(View.VISIBLE);
                    binding.rateButton.setVisibility(View.VISIBLE);
                    binding.textRatingBar.setVisibility(View.VISIBLE);
                    binding.editTextComment.setVisibility(View.VISIBLE);
                    binding.textComment.setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onFailure(Call<ProductRating> call, Throwable t) {

            }
        };
    }
}