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
    LocalData localData = LocalData.GetInstance();
    View view;
    Product product = null;
    Photo photo = null;
    ProductService productService = null;

    Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            .create();
    private final Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("http://10.0.2.2:61169/api/")
            // .baseUrl("https://10.0.2.2:44359/api/")
            .client(OkHttpClass.getUnsafeOkHttpClient().newBuilder().followRedirects(false).followSslRedirects(false).build())
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build();

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

                ProductRatingService apiService = retrofit.create(ProductRatingService.class);
                ProductRating productRating = new ProductRating();

                User user = new User();
                user.setId(localData.getUserId());
                productRating.setUser(user);

//                Product pp = new Product();
//                pp.setId(product.getId());
                productRating.setProduct(product);

                productRating.setStars((int) ratingbar.getRating());
                productRating.setComment(binding.textComment.getText().toString());

                //apiService.createProductRating(LocalData.GetInstance().getToken(),productRating);

                apiService.createProductRating(LocalData.GetInstance().getToken(),productRating).enqueue(new Callback<ProductRating>() {
                    @Override
                    public void onResponse(Call<ProductRating> call, Response<ProductRating> response) {
                        ProductRating check = response.body();

                    }

                    @Override
                    public void onFailure(Call<ProductRating> call, Throwable t) {
                        Log.v("FAIL TA MER" , "FAIL TA MER");
                    }
                });

                rateButton.setEnabled(false);
            }

        });
    }
}