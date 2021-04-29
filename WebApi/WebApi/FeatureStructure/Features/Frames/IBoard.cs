using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.Response;

namespace WebApi.FeatureStructure.Frames
{
    /// <summary>
    /// 畫面物件相關的介面
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// 依然面板物件及畫面更新決定應回傳的畫面物件
        /// </summary>
        /// <param name="boardObject">面板物件</param>
        /// <param name="frameUpdate">畫面更新</param>
        /// <returns>畫面物件</returns>
        FrameObject GetFrameObject(BoardObject boardObject, FrameUpdate frameUpdate);
    }
}
