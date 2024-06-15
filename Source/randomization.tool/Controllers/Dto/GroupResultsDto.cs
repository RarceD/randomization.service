//    Randomization Service
//    Copyright(C) 2024 Rubén Arce
//    and individual contributors.
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program. If not, see <https://www.gnu.org/licenses/>.
//
//

namespace randomization.tool.Controllers.Dto;
public class GroupResultsDto
{
    public string Name { get; set; }
    public List<string> Values { get; set; }
    public double Rank { get; set; }
    public double Mean { get; set; }
    public double SD { get; set; }
    public double Min { get; set; }
    public double Median { get; set; }
    public double Max { get; set; }
    public double pValue { get; set; }
}
