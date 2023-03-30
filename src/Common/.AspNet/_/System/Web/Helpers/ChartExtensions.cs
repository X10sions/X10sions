using System.Web.Mvc;

namespace System.Web.Helpers {
  public static class ChartExtensions {
    public static void WriteToImage(this Chart chart) { }

    public static HtmlString ToHtmlString(this Chart chart, string chartName, List<int[]> dataSource, string xTitle, string yTitle) => SetupHtml(chartName, GetDataSourceFromIntArray(dataSource), xTitle, yTitle);

    private static string GetDataSourceFromIntArray(List<int[]> dataSource) {
      Json.Encode(dataSource);
      return "arrDataSource = " + Json.Encode(dataSource);
    }

    private static HtmlString SetupHtml(string chartName, string dataSource, string xTitle, string yTitle) {
      var tagBuilder = new TagBuilder("div");
      tagBuilder.Attributes.Add("style", "border: solid 1px #F0F0F0;");
      var tagBuilder2 = new TagBuilder("canvas");
      tagBuilder2.Attributes.Add("id", chartName);
      tagBuilder2.Attributes.Add("height", "400");
      tagBuilder2.Attributes.Add("width", "600");
      tagBuilder2.Attributes.Add("style", "border: solid 1px red;");
      tagBuilder2.SetInnerText("Your browser does not support HTML5 Canvas");
      var tagBuilder3 = SetupScript(chartName, dataSource, xTitle, yTitle);
      var tagBuilder4 = new TagBuilder("noscript") {
        InnerHtml = "\r\nThis chart is unavailable because JavaScript is disabled on your computer.\r\nPlease enable JavaScript and refresh this page to see the chart in action."
      };
      tagBuilder.InnerHtml = tagBuilder2.ToString() + tagBuilder3.ToString() + tagBuilder4.ToString();
      return new HtmlString(tagBuilder.ToString());
    }

    private static TagBuilder SetupScript(string chartName, string dataSource, string xTitle, string yTitle) {
      var tagBuilder = new TagBuilder("script");
      tagBuilder.Attributes.Add("type", "text/javascript");
      tagBuilder.InnerHtml = Convert.ToString(Convert.ToString("\r\n<!--\r\n// chart sample data\r\nvar arrDataSource = new Array();\r\n") + dataSource + "\r\nvar canvas;\r\nvar context;\r\n// chart properties\r\nvar cWidth, cHeight, cMargin, cSpace;\r\nvar cMarginSpace, cMarginHeight;\r\n// bar properties\r\nvar bWidth, bMargin, totalBars, maxDataValue;\r\nvar bWidthMargin;\r\n// bar animation\r\nvar ctr, numctr, speed;\r\n// axis property\r\nvar totLabelsOnYAxis;\r\n\r\n\r\n// barchart constructor\r\nfunction barChart(data) {\r\nif(data!=null)\r\n{\r\narrDataSource = data;\r\n}\r\ncanvas = document.getElementById('") + chartName + "');\r\nif (canvas && canvas.getContext) {\r\ncontext = canvas.getContext('2d');\r\n}\r\n\r\nchartSettings();\r\ndrawAxisLabelMarkers();\r\ndrawChartWithAnimation();\r\n}\r\n\r\n// initialize the chart and bar values\r\nfunction chartSettings() {\r\n// chart properties\r\ncMargin = 25;\r\ncSpace = 60;\r\ncHeight = canvas.height - 2 * cMargin - cSpace;\r\ncWidth = canvas.width - 2 * cMargin - cSpace;\r\ncMarginSpace = cMargin + cSpace;\r\ncMarginHeight = cMargin + cHeight;\r\n// bar properties\r\nbMargin = 15;\r\ntotalBars = arrDataSource.length;\r\nbWidth = (cWidth / totalBars) - bMargin;\r\n\r\n\r\n// find maximum value to plot on chart\r\nmaxDataValue = 0;\r\nfor (var i = 0; i < totalBars; i++) {\r\nvar barVal = parseInt(arrDataSource[i][1]);\r\nif (parseInt(barVal) > parseInt(maxDataValue))\r\nmaxDataValue = barVal;\r\n}\r\n\r\ntotLabelsOnYAxis = 10;\r\ncontext.font = '10pt Garamond';\r\n\r\n// initialize Animation variables\r\nctr = 0;\r\nnumctr = 100;\r\nspeed = 10;\r\n}\r\n\r\n// draw chart axis, labels and markers\r\nfunction drawAxisLabelMarkers() {\r\ncontext.lineWidth = '2.0';\r\n// draw y axis\r\ndrawAxis(cMarginSpace, cMarginHeight, cMarginSpace, cMargin);\r\n// draw x axis\r\ndrawAxis(cMarginSpace, cMarginHeight, cMarginSpace + cWidth, cMarginHeight);\r\ncontext.lineWidth = '1.0';\r\ndrawMarkers();\r\n}\r\n\r\n// draw X and Y axis\r\nfunction drawAxis(x, y, X, Y) {\r\ncontext.beginPath();\r\ncontext.moveTo(x, y);\r\ncontext.lineTo(X, Y);\r\ncontext.closePath();\r\ncontext.stroke();\r\n}\r\n\r\n// draw chart markers on X and Y Axis\r\nfunction drawMarkers() {\r\nvar numMarkers = parseInt(maxDataValue / totLabelsOnYAxis);\r\ncontext.textAlign = 'right';\r\ncontext.fillStyle = '#000'; ;\r\n\r\n// Y Axis\r\nfor (var i = 0; i <= totLabelsOnYAxis; i++) {\r\nmarkerVal = i * numMarkers;\r\nmarkerValHt = i * numMarkers * cHeight;\r\nvar xMarkers = cMarginSpace - 5;\r\nvar yMarkers = cMarginHeight - (markerValHt / maxDataValue);\r\ncontext.fillText(markerVal, xMarkers, yMarkers, cSpace);\r\n}\r\n\r\n// X Axis\r\ncontext.textAlign = 'center';\r\nfor (var i = 0; i < totalBars; i++) {\r\n//arrval = arrDataSource[i].split(',');\r\n//name = arrval[0];\r\nname = arrDataSource[i][0];\r\n\r\nmarkerXPos = cMarginSpace + bMargin + (i * (bWidth + bMargin)) + (bWidth / 2);\r\nmarkerYPos = cMarginHeight + 10;\r\ncontext.fillText(name, markerXPos, markerYPos, bWidth);\r\n}\r\n\r\ncontext.save();\r\n\r\n// Add Y Axis title\r\ncontext.translate(cMargin + 10, cHeight / 2);\r\ncontext.rotate(Math.PI * -90 / 180);\r\ncontext.fillText('Visitors in Thousands', 0, 0);\r\n\r\ncontext.restore();\r\n\r\n// Add X Axis Title\r\ncontext.fillText('Year Wise', cMarginSpace + (cWidth / 2), cMarginHeight + 30);\r\n}\r\n\r\nfunction drawChartWithAnimation() {\r\n// Loop through the total bars and draw\r\nfor (var i = 0; i < totalBars; i++) {\r\n//var arrVal = arrDataSource[i].split(',');\r\n//bVal = parseInt(arrVal[1]);\r\nbVal = parseInt(arrDataSource[i][1]);\r\nbHt = (bVal * cHeight / maxDataValue) / numctr * ctr;\r\nbX = cMarginSpace + (i * (bWidth + bMargin)) + bMargin;\r\nbY = cMarginHeight - bHt - 2;\r\ndrawRectangle(bX, bY, bWidth, bHt, true);\r\n}\r\n\r\n// timeout runs and checks if bars have reached the desired height\r\n// if not, keep growing\r\nif (ctr < numctr) {\r\nctr = ctr + 1;\r\nsetTimeout(arguments.callee, speed);\r\n}\r\n}\r\n\r\nfunction drawRectangle(x, y, w, h, fill) {\r\ncontext.beginPath();\r\ncontext.rect(x, y, w, h);\r\ncontext.closePath();\r\ncontext.stroke();\r\n\r\nif (fill) {\r\nvar gradient = context.createLinearGradient(0, 0, 0, 300);\r\ngradient.addColorStop(0, 'green');\r\ngradient.addColorStop(1, 'rgba(67,203,36,.15)');\r\ncontext.fillStyle = gradient;\r\ncontext.strokeStyle = gradient;\r\ncontext.fill();\r\n}\r\n}\r\n-->\r\n";
      return tagBuilder;
    }
  }
}
