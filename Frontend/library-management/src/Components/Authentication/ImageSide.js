import React from 'react';
import BuildingA from '../../Assets/Images/Img_BuildingA_Tdu.png';
import "../../Assets/Styles/Authentication/ImageSide.css"


const ImageSide = () => {
  return (
    <div className="Image-Side">
      <img src={BuildingA} alt="Building A" className="BuildingA-Image" />
    </div>
  );
};

export default ImageSide;
