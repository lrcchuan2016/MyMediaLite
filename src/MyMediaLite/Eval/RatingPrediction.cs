// Copyright (C) 2010 Zeno Gantner
//
// This file is part of MyMediaLite.
//
// MyMediaLite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// MyMediaLite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MyMediaLite.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Globalization;
using System.IO;
using MyMediaLite.Data;
using MyMediaLite.RatingPrediction;

namespace MyMediaLite.eval
{
	/// <summary>Class that contains static methods for rating prediction</summary>
	public class RatingPrediction
	{
		/// <summary>Rates a given set of instances</summary>
		/// <param name="engine">rating prediction engine</param>
		/// <param name="ratings">test cases</param>
		/// <param name="user_mapping">an <see cref="EntityMapping"/> object for the user IDs</param>
		/// <param name="item_mapping">an <see cref="EntityMapping"/> object for the item IDs</param>
		/// <param name="writer">the TextWriter to write the predictions to</param>
		public static void WritePredictions(
			IRatingPredictor engine,
			RatingData ratings,
			EntityMapping user_mapping, EntityMapping item_mapping,		                                    
			TextWriter writer)
		{
			var ni = new NumberFormatInfo();
			ni.NumberDecimalDigits = '.';

			foreach (RatingEvent r in ratings)
				writer.WriteLine("{0}\t{1}\t{2}",
								 user_mapping.ToOriginalID(r.user_id),
								 item_mapping.ToOriginalID(r.item_id),
								 engine.Predict(r.user_id, r.item_id).ToString(ni));
		}

		/// <summary>Rates a given set of instances</summary>
		/// <param name="engine">rating prediction engine</param>
		/// <param name="ratings">test cases</param>
		/// <param name="user_mapping">an <see cref="EntityMapping"/> object for the user IDs</param>
		/// <param name="item_mapping">an <see cref="EntityMapping"/> object for the item IDs</param>
		/// <param name="filename">the name of the file to write the predictions to</param>
		public static void WritePredictions(
			IRatingPredictor engine,
			RatingData ratings,
			EntityMapping user_mapping, EntityMapping item_mapping,
			string filename)
		{
			if (filename.Equals("-"))
				WritePredictions(engine, ratings, user_mapping, item_mapping, Console.Out);
			else
				using ( StreamWriter writer = new StreamWriter(filename) )
				{
					WritePredictions(engine, ratings, user_mapping, item_mapping, writer);
				}
		}
	}
}
