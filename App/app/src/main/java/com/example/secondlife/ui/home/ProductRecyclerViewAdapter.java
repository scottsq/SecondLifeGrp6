package com.example.secondlife.ui.home;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.R;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.squareup.picasso.Picasso;

import java.io.InputStream;
import java.util.List;

public class ProductRecyclerViewAdapter  extends RecyclerView.Adapter<ProductRecyclerViewAdapter.ProdcutViewHolder>{
    private List<Product> dataSetProduct;
    private List<Photo> dataSetPhoto;
    private Context context;

    public ProductRecyclerViewAdapter(List<Product> products, List<Photo> photos, Context context) {
        this.dataSetProduct = products;
        this.dataSetPhoto = photos;
        this.context = context;
    }

    @NonNull
    @Override
    public ProdcutViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(parent.getContext()).inflate(R.layout.recyclerview_product_item, parent, false);
        ProdcutViewHolder holder = new ProdcutViewHolder(itemView);
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull ProdcutViewHolder holder, final int position) {
        Picasso.get().load(dataSetPhoto.get(position).getUrl()).placeholder(R.drawable.ic_baseline_image_search_24).into(holder.getImageView());
        holder.getNameView().setText(dataSetProduct.get(position).getName());
        holder.getPriceView().setText(dataSetProduct.get(position).getPrice() + "€");
        holder.getBtnView().setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Toast.makeText(context, "Clicked on " + dataSetProduct.get(position).getName(), Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    public int getItemCount() {
        return dataSetProduct.size();
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

    private class DownloadImageTask extends AsyncTask<String, Void, Bitmap> {
        ImageView bmImage;

        public DownloadImageTask(ImageView bmImage) {
            this.bmImage = bmImage;
        }

        protected Bitmap doInBackground(String... urls) {
            String urldisplay = urls[0];
            Bitmap mIcon11 = null;
            try {
                InputStream in = new java.net.URL(urldisplay).openStream();
                mIcon11 = BitmapFactory.decodeStream(in);
            } catch (Exception e) {
                Log.e("Error", e.getMessage());
                e.printStackTrace();
            }
            return mIcon11;
        }

        protected void onPostExecute(Bitmap result) {
            bmImage.setImageBitmap(result);
        }
    }
}
