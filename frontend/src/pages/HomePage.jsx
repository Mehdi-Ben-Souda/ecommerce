import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import ProductCard from "../components/ProductCard";

const HomePage = () => {
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get(`http://172.20.1.42:5000/catalog`)
      .then((response) => {
        console.log(response.data);
        setProducts(response.data);
      })
      .catch((error) => {
        if (error.response) {
          // Server responded with error status
          console.error("Response error:", error.response.status);
        } else if (error.request) {
          // Request made but no response
          console.error("Network error - no response");
        } else {
          // Error in request setup
          console.error("Error:", error.message);
        }
      });
  }, []);

  const handleProductClick = (product) => {
    navigate(`/product`, { state: { product } });
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-4xl font-bold text-center mb-8">
        Welcome to the Home e-commerce
      </h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
        {products.map((product) => (
          <ProductCard
            key={product._id}
            product={product}
            onClick={() => handleProductClick(product)}
          />
        ))}
      </div>
    </div>
  );
};

export default HomePage;
