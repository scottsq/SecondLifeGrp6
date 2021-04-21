package com.example.secondlife.model;

public class ProductRating {
    private int Id;
    private Product Product;
    private User User;
    private int Stars;
    private String Comment;

    public int getId() {
        return Id;
    }

    public void setId(int id) {
        Id = id;
    }

    public Product getProduct() {
        return Product;
    }

    public void setProduct(Product product) {
        Product = product;
    }

    public User getUser() {
        return User;
    }

    public void setUser(User user) {
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
