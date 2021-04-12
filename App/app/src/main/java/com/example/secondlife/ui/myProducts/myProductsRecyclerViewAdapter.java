package com.example.secondlife.ui.myProducts;

import android.content.Context;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.fragment.app.FragmentActivity;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.ProductWithPhoto;
import com.example.secondlife.network.ProductRatingService;
import com.example.secondlife.network.ProductService;
import com.example.secondlife.ui.home.ProductRecyclerViewAdapter;
import com.squareup.picasso.Picasso;

import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class myProductsRecyclerViewAdapter extends RecyclerView.Adapter<myProductsRecyclerViewAdapter.ProductViewHolder>{
    private List<ProductWithPhoto> dataSetProduct;
    private List<Photo> dataSetPhoto;
    private Context context;
    private List<myProductsRecyclerViewAdapter.ProductViewHolder> dataSetHolder = new ArrayList<>();
    private FragmentActivity fragmentActivity;
    private ViewGroup parent;

    private LocalData localData = LocalData.GetInstance();
    private Retrofit retrofit = localData.GetRetrofit();
    private ProductService apiService = retrofit.create(ProductService.class);

    public myProductsRecyclerViewAdapter(FragmentActivity f, List<ProductWithPhoto> products, Context context) {
        this.fragmentActivity = f;
        this.dataSetProduct = products;
        this.context = context;
    }

    public myProductsRecyclerViewAdapter.ProductViewHolder getHolder(int position) {
        return dataSetHolder.get(position);
    }

    @NonNull
    @Override
    public myProductsRecyclerViewAdapter.ProductViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(parent.getContext()).inflate(R.layout.recyclerview_product_item, parent, false);
        myProductsRecyclerViewAdapter.ProductViewHolder holder = new myProductsRecyclerViewAdapter.ProductViewHolder(itemView);
        this.parent = parent;
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull myProductsRecyclerViewAdapter.ProductViewHolder holder, final int position) {
        //String url = dataSetPhoto.get(position).getUrl();
        //Picasso.get().load(url).placeholder(R.drawable.ic_baseline_image_search_24).into(holder.getImageView());
        //si l'image est null
        if (dataSetProduct.get(position).getPhotoList() == null || dataSetProduct.get(position).getPhotoList().size() == 0 ){
            Picasso.get().load(R.drawable.ic_baseline_image_search_24).placeholder(R.drawable.ic_baseline_image_search_24).into(holder.getImageView());
        }
        else
        {
            Picasso.get().load(dataSetProduct.get(position).getPhotoList().get(0).getUrl()).placeholder(R.drawable.ic_baseline_image_search_24).into(holder.getImageView());
        }

        holder.getNameView().setText(dataSetProduct.get(position).getProduct().getName());
        holder.getPriceView().setText(dataSetProduct.get(position).getProduct().getPrice() + "€");
        holder.getBtnView().setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                apiService.deleteProduct(localData.getToken(), dataSetProduct.get(position).getProduct().getId()).enqueue(deleteProduct());
            }
        });
        dataSetHolder.add(holder);
    }

    @Override
    public int getItemCount() {
        return dataSetProduct.size();
    }

    public int GetHoldersCount() {
        return dataSetHolder.size();
    }

    private Callback<Void> deleteProduct(){
        return new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {
                Toast.makeText(parent.getContext(),response.isSuccessful()? "Produit supprimé" : "Erreur", Toast.LENGTH_SHORT);
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {

            }
        };
    }


    public static class ProductViewHolder extends RecyclerView.ViewHolder {

        private final TextView name;
        private final TextView price;
        private final Button btn;
        private final ImageView img;

        public ProductViewHolder(@NonNull View itemView) {
            super(itemView);
            img = itemView.findViewById(R.id.item_image);
            name = itemView.findViewById(R.id.item_name);
            price = itemView.findViewById(R.id.item_price);
            btn = itemView.findViewById(R.id.item_more);
        }

        public TextView getNameView() {
            return name;
        }

        public TextView getPriceView() {
            return price;
        }

        public Button getBtnView() {
            return btn;
        }

        public ImageView getImageView() {
            return img;
        }
    }
}
