package com.example.secondlife.ui.home;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.R;
import com.example.secondlife.model.Product;

import java.util.List;

public class ProductRecyclerViewAdapter  extends RecyclerView.Adapter<ProductRecyclerViewAdapter.ProdcutViewHolder>{
    private List<Product> products;

    public ProductRecyclerViewAdapter(List<Product> products) {
        this.products = products;
    }

    @NonNull
    @Override
    public ProdcutViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(parent.getContext()).inflate(R.layout.recyclerview_product_item, parent, false);
        ProdcutViewHolder holder = new ProdcutViewHolder(itemView);
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull ProdcutViewHolder holder, int position) {
        Product product = products.get(position);
        holder.bind(product);
    }

    @Override
    public int getItemCount() {
        return products.size();
    }

    static class ProdcutViewHolder extends RecyclerView.ViewHolder {

        private final TextView name;
        //private final TextView age;

        public ProdcutViewHolder(@NonNull View itemView) {
            super(itemView);
            name = itemView.findViewById(R.id.item_name);
            //age = itemView.findViewById(R.id.item_age);
        }

        public void bind(Product product) {
            name.setText(product.getName());
            //age.setText(String.valueOf(product.getAge()));
        }
    }
}
