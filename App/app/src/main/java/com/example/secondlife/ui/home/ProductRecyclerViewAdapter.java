package com.example.secondlife.ui.home;

import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentActivity;
import androidx.fragment.app.FragmentManager;
import androidx.fragment.app.FragmentTransaction;
import androidx.navigation.NavDirections;
import androidx.navigation.Navigation;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.MainActivity;
import com.example.secondlife.R;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.ui.ProductDetails;
import com.example.secondlife.ui.productDetails.ProductDetailsFragment;
import com.google.gson.Gson;
import com.squareup.picasso.Picasso;

import org.json.JSONObject;

import java.io.InputStream;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;


public class ProductRecyclerViewAdapter  extends RecyclerView.Adapter<ProductRecyclerViewAdapter.ProdcutViewHolder>{
    private List<Product> dataSetProduct;
    private List<Photo> dataSetPhoto;
    private Context context;
    private List<ProdcutViewHolder> dataSetHolder = new ArrayList<>();
    private FragmentActivity fragmentActivity;
    private ViewGroup parent;

    public ProductRecyclerViewAdapter(FragmentActivity f, List<Product> products, List<Photo> photos, Context context) {
        this.fragmentActivity = f;
        this.dataSetProduct = products;
        this.dataSetPhoto = photos;
        this.context = context;
    }

    public ProdcutViewHolder getHolder(int position) {
        return dataSetHolder.get(position);
    }

    @NonNull
    @Override
    public ProdcutViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(parent.getContext()).inflate(R.layout.recyclerview_product_item, parent, false);
        ProdcutViewHolder holder = new ProdcutViewHolder(itemView);
        this.parent = parent;
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull ProdcutViewHolder holder, final int position) {
        //String url = dataSetPhoto.get(position).getUrl();
        //Picasso.get().load(url).placeholder(R.drawable.ic_baseline_image_search_24).into(holder.getImageView());
        Picasso.get().load(R.drawable.ic_baseline_image_search_24).placeholder(R.drawable.ic_baseline_image_search_24).into(holder.getImageView());
        holder.getNameView().setText(dataSetProduct.get(position).getName());
        holder.getPriceView().setText(dataSetProduct.get(position).getPrice() + "€");
        holder.getBtnView().setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

//                FragmentTransaction ft = fragmentActivity.getSupportFragmentManager().beginTransaction().setTransition(FragmentTransaction.TRANSIT_FRAGMENT_OPEN);
//                //ft.setReorderingAllowed(true);
//                ProductDetailsFragment productDetailsFragment = new ProductDetailsFragment();
                Bundle bundle = new Bundle();
                bundle.putSerializable("product", (Serializable)dataSetProduct.get(position));
//               // bundle.putSerializable("photo", (Serializable)dataSetPhoto.get(position));
//                productDetailsFragment.setArguments(bundle);
//                ft.replace(R.id.nav_host_fragment, productDetailsFragment);
//                ft.addToBackStack(null);
//                ft.commit();

                Navigation.findNavController(v).navigate(R.id.action_navigation_home_to_navigation_product_details, bundle);



//                Intent i = new Intent(parent.getContext(), ProductDetails.class);
//                Gson gson = new Gson();
//                String product = gson.toJson(dataSetProduct.get(position));
//                //String photo = gson.toJson(dataSetPhoto.get(position));
//                Photo photoTest = new Photo();
//                photoTest.setUrl("");
//                String photo = gson.toJson(photoTest);
//                i.putExtra("product", product);
//                i.putExtra("photo", photo);
//                parent.getContext().startActivity(i);
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





    public static class ProdcutViewHolder extends RecyclerView.ViewHolder {

        private final TextView name;
        private final TextView price;
        private final Button btn;
        private final ImageView img;

        public ProdcutViewHolder(@NonNull View itemView) {
            super(itemView);
            img = (ImageView) itemView.findViewById(R.id.item_image);
            name = (TextView) itemView.findViewById(R.id.item_name);
            price = (TextView) itemView.findViewById(R.id.item_price);
            btn = (Button) itemView.findViewById(R.id.item_more);
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
