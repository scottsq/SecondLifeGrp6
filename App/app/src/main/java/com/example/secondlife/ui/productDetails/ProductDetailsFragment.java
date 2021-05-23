package com.example.secondlife.ui.productDetails;

import androidx.fragment.app.FragmentTransaction;
import androidx.lifecycle.Observer;
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

    public static ProductDetailsFragment newInstance() {
        return new ProductDetailsFragment();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {

        Bundle bundle = getArguments();
        product = (ProductWithPhoto) bundle.getSerializable("product");

        binding = ProductDetailsFragmentBinding.inflate(inflater, container, false);
        view = binding.getRoot();
        mViewModel = new ViewModelProvider(getActivity()).get(ProductDetailsViewModel.class);

        // Rating
        callRateButton();

        // Pour les info du Product
        initProductInfo();

        if (localData.getUserId() > -1) {
            mViewModel.getUserRating(product.getProduct().getId(),localData.getUserId());
            // Vérifie si l'utilisateur à voté avec l'appel API
            //S'il a voté : dans la fonction getUserRating --> isSuccessful ok et les champs seront caché ( car de base ils sont cachés)
            //S'il n'a pas voté :dans la fonction getUserRating --> isSuccessful pas ok / error 400 bad request  et les champs seront affichés
            Observer<ProductRating> o = productRating -> {
                if (productRating != null) return;
                binding.ratingBar.setVisibility(View.VISIBLE);
                binding.rateButton.setVisibility(View.VISIBLE);
                binding.textRatingBar.setVisibility(View.VISIBLE);
                binding.editTextComment.setVisibility(View.VISIBLE);
                binding.textComment.setVisibility(View.VISIBLE);
            };
            mViewModel.getProductRatingMutableLiveData().observe(getActivity(), o);

            binding.btnBuy.setVisibility(View.VISIBLE);
        }

        return view;
    }

    private void initProductInfo() {
        if (product.getPhotoList() == null || product.getPhotoList().size() == 0){
            Picasso.get().load(R.drawable.ic_baseline_image_search_24).placeholder(R.drawable.ic_baseline_image_search_24).into(binding.productImg);
        }
        else
        {
            Picasso.get().load(product.getPhotoList().get(0).getUrl()).placeholder(R.drawable.ic_baseline_image_search_24).into(binding.productImg);
        }
        binding.productName.setText(product.getProduct().getName());
        binding.productDesc.setText(product.getProduct().getDescription());

        mViewModel.getProductAverage(product.getProduct().getId());
        Observer<Float> o = value -> {
            if (binding != null)
            {
                binding.textStars.setText("Note: " + value +" ⭐");
            }
        };
        mViewModel.getAverageLiveData().observe(getActivity(), o);

        binding.textOwner.setText("Vendeur: " + product.getProduct().getOwner().getName());
        binding.textPrice.setText("Prix: " + product.getProduct().getPrice() + "€");
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

    public void callRateButton(){
        RatingBar ratingbar = binding.ratingBar;
        Button rateButton = binding.rateButton;

        rateButton.setOnClickListener(arg0 -> {
            User user = new User();
            user.setId(localData.getUserId());

            ProductRating productRating = new ProductRating();
            productRating.setUser(user);
            productRating.setProduct(product.getProduct());
            productRating.setStars((int) ratingbar.getRating());
            productRating.setComment(binding.editTextComment.getText().toString());

            mViewModel.createProductRating(productRating);
            rateButton.setEnabled(false);
        });
    }


}