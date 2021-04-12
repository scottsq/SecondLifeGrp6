package com.example.secondlife.model;

import java.io.Serializable;
import java.util.List;

public class ProductWithPhoto implements Serializable {
    private Product product;
    private List<Photo> photos;

    public Product getProduct() {
        return product;
    }

    public void setProduct(Product product) {
        this.product = product;
    }

    public List<Photo> getPhotoList() {
        return photos;
    }

    public void setPhotoList(List<Photo> photos) {
        this.photos = photos;
    }

}
