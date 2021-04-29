using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.FeatureStructure.Computes
{
    /// <summary>
    /// 計算物件相關的介面
    /// </summary>
    public interface ICompute
    {
        /// <summary>
        /// 依計算物件回傳畫面更新
        /// </summary>
        /// <param name="computeObject">計算物件</param>
        /// <returns>畫面更新</returns>
        FrameUpdate Compute(ComputeObject computeObject);
    }
}