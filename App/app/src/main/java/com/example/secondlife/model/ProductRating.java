package com.example.secondlife.model;

public class ProductRating {
    public int Id;
    public Product Product;
    public User User;
    public int Stars;
    public String Comment;

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }

    public com.example.secondlife.model.Product getProduct() {
        return Product;
    }

    public void setProduct(com.example.secondlife.model.Product product) {
        Product = product;
    }

    public com.example.secondlife.model.User getUser() {
        return User;
    }

    public void setUser(com.example.secondlife.model.User user) {
        User = user;
    }

    public int getStars() {
        return Stars;
    }

    public void setStars(int stars) {
        Stars = stars;
    }

    public String getComment() {
        return Comment;
    }

    public void setComment(String comment) {
        Comment = comment;
    }

}
